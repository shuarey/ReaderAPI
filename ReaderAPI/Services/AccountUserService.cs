﻿using System.Net;
using System.Runtime.CompilerServices;
using Dapper;
using Microsoft.AspNetCore.Http.Extensions;
using ReaderAPI.Infrastructure;
using ReaderAPI.Middleware;
using ReaderAPI.Models;
using Serilog.Context;
using static ReaderAPI.Models.BaseClasses;

namespace ReaderAPI.Services
{
    public class AccountUserService : BaseService
    {
        public AccountUserService ( IHttpContextAccessor context, IDatabaseConnection connection, ILogger<AccountUserService> logger, ILoggerFactory loggerFactory ) : base ( context, connection, logger, loggerFactory ) { }

        static AccountUserService ( )
        {
            SqlMapper.SetTypeMap ( typeof ( AccountUser ), new CustomPropertyTypeMap (
                typeof ( AccountUser ), ( type, columnName ) =>
                    type.GetProperties ( ).FirstOrDefault ( prop =>
                        prop.Name.Equals ( columnName, StringComparison.OrdinalIgnoreCase ) || 
                          ( columnName == "FIRST_NAME" && prop.Name == "FirstName" ) || 
                          ( columnName == "LAST_NAME" && prop.Name == "LastName" ) || 
                          ( columnName == "FULL_NAME" && prop.Name == "FullName" ) ||
                          ( columnName == "ACCT_LOCK_TS" && prop.Name == "LockedTimestamp" ) )
            ) );

            SqlMapper.SetTypeMap ( typeof ( Login ), new CustomPropertyTypeMap (
                typeof ( Login ), ( type, columnName ) =>
                    type.GetProperties ( ).FirstOrDefault ( prop =>
                        prop.Name.Equals ( columnName, StringComparison.OrdinalIgnoreCase ) ||
                          ( columnName == "ACCT_USER_ID" && prop.Name == "AcctUserID" ) ||
                          ( columnName == "LOGIN_TIMESTAMP" && prop.Name == "LoginTs" ) ||
                          ( columnName == "IS_SUCCESSFUL" && prop.Name == "IsSuccessful" ) ||
                          ( columnName == "IP_ADDRESS" && prop.Name == "IPAddress" ) )
            ) );
        }

        public BaseResponse LoginUser ( AccountUserLoginPOSTRequest request )
        {
            AccountUser user = null;
            BasicErrorResponse errorResponse = null;
            try
            {
                errorResponse = GetUserLoginID ( request.email, ref user, request.password );
                if ( errorResponse != null )
                    return errorResponse;

                if ( user == null )
                    return new BasicErrorResponse ( "User not found.", HttpStatusCode.InternalServerError );


                string absolutePath = new Uri ( Context.HttpContext.Request.GetDisplayUrl ( ) ).AbsolutePath;

                return new AccountUserPOSTResponse
                {
                    success = true,
                    message = "Login successful",
                    id = user.ID,
                    first_name = user.FirstName,
                    last_name = user.LastName
                };
            }
            catch ( Exception ex )
            {
                return new BasicErrorResponse ( ex.Message, HttpStatusCode.InternalServerError );
            }
            finally
            {
                if ( user != null )
                {
                    var query = @"INSERT INTO LOGIN_HISTORY (ID, ACCT_USER_ID, LOGIN_TIMESTAMP, IS_SUCCESSFUL, IP_ADDRESS)
                                VALUES (@ID, @AcctUserID, @LoginTs, @IsSuccessful, @IPAddress)";

                    var IPAddress = Context.HttpContext.Connection.RemoteIpAddress?.ToString ( );
                    var parameters = new
                    {
                        ID = Guid.NewGuid ( ).ToString ( ),
                        AcctUserID = user.ID,
                        LoginTs = DateTime.UtcNow,
                        IsSuccessful = errorResponse == null,
                        IPAddress = IPAddress
                    };

                    Connection.Execute ( query, parameters );
                }
            }
        }

        public BaseResponse RegisterAccountUser ( AccountUserRegisterPOSTRequest request )
        {
            try
            {
                BasicErrorResponse response = null;
                AccountUser user = null;
                string accountUserID = string.Empty;
                response = GetUserLoginID ( request.email, ref user );
                if ( response != null )
                    return response;

                if ( user != null )
                    return new BasicErrorResponse ( "email already associated with existing account.", HttpStatusCode.Conflict );

                var query = @"INSERT INTO ACCOUNT_USER (ID, FIRST_NAME, LAST_NAME, EMAIL, PASSWORD, CREATE_TS)
                            VALUES (@ID, @FirstName, @LastName, @Email, @Password, @CreateTs)";

                var passwordHash = BCrypt.Net.BCrypt.HashPassword ( request.password );
                var parameters = new
                {
                    ID = Guid.NewGuid ( ).ToString ( ),
                    FirstName = request.first_name,
                    LastName = request.last_name,
                    Email = request.email,
                    Password = passwordHash,
                    CreateTs = DateTime.UtcNow
                };
                accountUserID = parameters.ID;

                // Execute the query
                Connection.Execute ( query, parameters );

                return new AccountUserPOSTResponse { id = accountUserID, success = true, message = "success" };
            }
            catch ( Exception ex )
            {
                return new BasicErrorResponse ( ex.Message, HttpStatusCode.InternalServerError );
            }
        }

        private BasicErrorResponse GetUserLoginID ( string email, ref AccountUser user, string password = "" )
        {
            var query = "SELECT * FROM ACCOUNT_USER WHERE EMAIL = @Email";
            var dictFieldValue = new Dictionary<string, object> { { "Email", email } };

            user = Connection.QueryFirstOrDefault<AccountUser> ( query, dictFieldValue );
            if ( user != null )
            {
                if ( !string.IsNullOrEmpty ( password ) )
                {
                    var isValid = BCrypt.Net.BCrypt.Verify ( password, user.Password );
                    if ( !isValid )
                    {
                        var loginAttempts = Connection.Query<int> (
                            "SELECT COUNT(*) FROM LOGIN_HISTORY WHERE ACCT_USER_ID = @AcctUserID AND IS_SUCCESSFUL = 0 AND LOGIN_TIMESTAMP > DATEADD(HOUR, -1, GETUTCDATE())",
                            new { AcctUserID = user.ID } ).FirstOrDefault ( );

                        if ( loginAttempts >= 5 )
                        {
                            Connection.Execute (
                                "UPDATE ACCOUNT_USER SET ACCT_LOCK_TS = @LockTs WHERE ID = @AcctUserID",
                                new { LockTs = DateTime.UtcNow, AcctUserID = user.ID } );

                            return new BasicErrorResponse ( "Account locked due to too many failed login attempts.", HttpStatusCode.Forbidden );
                        }
                        return new BasicErrorResponse ( "invalid password", HttpStatusCode.BadRequest );
                    }

                    if ( user.LockedTimestamp != DateTime.MinValue )
                    {
                        var lockDuration = DateTime.UtcNow - user.LockedTimestamp;
                        if ( lockDuration.TotalMinutes < 60 )
                            return new BasicErrorResponse ( "Account locked. Please try again later.", HttpStatusCode.Forbidden );
                        else
                            Connection.Execute (
                                "UPDATE ACCOUNT_USER SET ACCT_LOCK_TS = NULL WHERE ID = @AcctUserID",
                                new { AcctUserID = user.ID } );
                    }
                }
            }

            return null;
        }
    }
}
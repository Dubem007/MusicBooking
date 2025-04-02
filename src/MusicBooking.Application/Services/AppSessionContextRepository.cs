using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;
using MusicBooking.Application.Interface;
using MusicBooking.Domain.Dtos;
using MusicBooking.Domain.Entites;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicBooking.Application.Services
{
    public class AppSessionContextRepository: IAppSessionContextRepository
    {
        private readonly IHttpContextAccessor _contextAccessor;

        public AppSessionContextRepository(IHttpContextAccessor contextAccessor)
        {
            _contextAccessor = contextAccessor;
        }

        public async Task<UserDto> GetUserDetails()
        {
            var tokenUser = ValidateJwt(_contextAccessor.HttpContext);
            return await Task.Run(() => tokenUser);
        }
        private static UserDto ValidateJwt(HttpContext context)
        {
            var appUser = new UserDto();
            try
            {
                var authValues = context.Request.Headers["Authorization"];
                if (StringValues.IsNullOrEmpty(authValues) || !authValues.Any(m => m.StartsWith("Bearer", StringComparison.OrdinalIgnoreCase)))
                {
                    return new UserDto();
                }
                var authHeader = authValues.FirstOrDefault(m => m != null && m.StartsWith("Bearer"));

                if (authHeader != null)
                {
                    string userPermissions = string.Empty;
                    var userGroupPermissions = string.Empty;
                    var roles = string.Empty;
                    string userAccountPermissions = string.Empty;
                    string groupAccountPermissions = string.Empty;

                    string authToken = !string.IsNullOrWhiteSpace(authHeader) ? authHeader.Split(" ")[1] : string.Empty;
                    var jwtoken = new JwtSecurityTokenHandler().ReadJwtToken(authToken);
                    Dictionary<string, string> tokenValues = new Dictionary<string, string>();
                    foreach (var claim in jwtoken.Claims)
                    {
                        if (!tokenValues.ContainsKey(claim.Type))
                        {
                            tokenValues.Add(claim.Type, claim.Value);
                        }
                        else
                        {
                            tokenValues[claim.Type] = tokenValues[claim.Type] + "," + claim.Value;
                        }
                    }
                    if (tokenValues.Any())
                    {
                        if (tokenValues.Any())
                        {
                            
                            appUser.Email = tokenValues["Email"];

                            if (tokenValues.ContainsKey("PhoneNumber"))
                            {
                                var usrName = tokenValues["PhoneNumber"];
                                appUser.PhoneNumber = usrName;
                            }
                            else
                            {
                                Console.WriteLine("PhoneNumber key not found in tokenValues dictionary.");
                            }

                            if (tokenValues.ContainsKey("UserId"))
                            {
                                var usrId = tokenValues["UserId"];
                                appUser.UserId = Guid.Parse(usrId);
                            }
                            else
                            {
                                Console.WriteLine("User Id key not found in tokenValues dictionary.");
                            }

                            if (tokenValues.ContainsKey("IsOrganizer"))
                            {
                                var isOrganizer = tokenValues["IsOrganizer"];
                                appUser.IsOrganizer = Convert.ToBoolean(isOrganizer);
                            }
                            else {
                                Console.WriteLine("IsOrganizer key not found in tokenValues dictionary.");
                            }
                        }
                        else
                        {
                            return new UserDto();
                        }
                    }
                    else
                    {
                        return new UserDto();
                    }
                }
                else { return new UserDto(); }
            }
            catch (Exception ex)
            {
                return new UserDto();
            }
            return appUser;
        }
    }
}

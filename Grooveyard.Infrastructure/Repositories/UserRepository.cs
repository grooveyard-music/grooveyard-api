
using Grooveyard.Domain.Interfaces.Repositories.User;
using Grooveyard.Domain.Models.User;
using Grooveyard.Infrastructure.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;


namespace Grooveyard.Infrastructure.Repositories
{
    public class UserRepository : IUserRepository
    {

        private readonly GrooveyardDbContext _context;

        public UserRepository(GrooveyardDbContext context)
        {
            _context = context;

        }

        public async Task<UserProfile> GetUserProfile(string userId)
        {
            try
            {
                var userProfile = _context.UserProfiles.FirstOrDefault(x => x.UserId == userId);
                return userProfile;
            }
            catch (Exception ex)
            {
                throw new Exception("Could not retrieve user profile", ex);
            }
        }

        public async Task<List<UserProfile>> GetUserProfilesByIds(List<string> userIds)
        {
            try
            {
                var userProfiles = new List<UserProfile>();
                foreach (var userId in userIds)
                {
                    var userProfile = _context.UserProfiles.FirstOrDefault(x => x.UserId == userId);
                    if (userProfile != null)
                    {
                        userProfiles.Add(userProfile);
                    }
                }

                return userProfiles;
            }
            catch (Exception ex)
            {
                throw new Exception("Could not retrieve user profiles", ex);
            }
        }

        public async Task<List<IdentityUser>> GetUsersByIds(List<string> userIds)
        {
            try
            {
                var usernames = new List<IdentityUser>();
                foreach (var userId in userIds)
                {
                    var user = _context.Users.FirstOrDefault(x => x.Id == userId);
                    usernames.Add(user);
                }

                return usernames;
            }
            catch (Exception ex)
            {
                throw new Exception("Could not retrieve user profile", ex);
            }
        }

        public async Task<IdentityUser> GetUserById(string userId)
        {
            try
            {

               var user = _context.Users.FirstOrDefault(x => x.Id == userId);
              
                return user;
            }
            catch (Exception ex)
            {
                throw new Exception("Could not retrieve user", ex);
            }
        }

        public async Task<UserProfile> CreateUserProfile(UserProfile userProfile)
        {
            try
            {
                _context.UserProfiles.Add(userProfile);
                await _context.SaveChangesAsync();

                return userProfile;
            }
            catch (Exception ex)
            {
                throw new Exception("Could not create user profile", ex);
            }
        }

        public async Task<UserProfile> UpdateUserProfile(UserProfile userProfile)
        {
            try
            {
                _context.Entry(userProfile).State = EntityState.Modified;
                await _context.SaveChangesAsync();
                return userProfile;
            }
            catch (Exception ex)
            {
                throw new Exception("Could not create user profile", ex);
            }
        }

        public async Task<RefreshToken> GetRefreshTokenByToken(string refreshToken)
        {
            return await _context.RefreshTokens.FirstOrDefaultAsync(rt => rt.Token == refreshToken);
        }

        public async Task<RefreshToken> GetRefreshToken(string userId)
        {
            try
            {
                var tokenEntry = await _context.RefreshTokens
                                               .FirstOrDefaultAsync(rt => rt.UserId == userId);
                if (tokenEntry != null && tokenEntry.ExpirationDate > DateTime.UtcNow && !tokenEntry.Revoked)
                {
                    return tokenEntry;
                }
                return null;
            }
            catch (Exception ex)
            {
                throw new Exception("Could not retrieve refresh token", ex);
            }
        }

        public async Task StoreRefreshToken(RefreshToken refreshToken)
        {
            try
            {
                // Check if the user already has a refresh token
                var existingToken = await _context.RefreshTokens
                                                  .FirstOrDefaultAsync(rt => rt.UserId == refreshToken.UserId);
                if (existingToken != null)
                {
                    // Update the existing token details
                    existingToken.Token = refreshToken.Token;
                    existingToken.ExpirationDate = refreshToken.ExpirationDate;
                    existingToken.CreationDate = refreshToken.CreationDate;
                    existingToken.Revoked = false;
                }
                else
                {
                    // Create a new refresh token entry
                    _context.RefreshTokens.Add(refreshToken);
                }

                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("Could not store refresh token", ex);
            }
        }

        public async Task UpdateRefreshToken(RefreshToken refreshToken)
        {
            try
            {
                _context.Entry(refreshToken).State = EntityState.Modified;
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                // Pseudo-logging
                Console.WriteLine($"Error updating refresh token: {ex.Message}");
                throw new Exception("Error updating refresh token.", ex);
            }
        }

        public async Task RevokeRefreshToken(string userId)
        {
            try
            {
                var tokenEntry = await _context.RefreshTokens
                                               .FirstOrDefaultAsync(rt => rt.UserId == userId);
                if (tokenEntry != null)
                {
                    _context.RefreshTokens.Remove(tokenEntry);
                    await _context.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Could not revoke refresh token", ex);
            }
        }

        public async Task<bool> CheckDisplayName(string displayName)
        {
            try
            {
                var displayNameExists = _context.UserProfiles.Any(p => p.DisplayName == displayName);

                return displayNameExists;

            }
            catch (Exception ex)
            {
                throw new Exception("Could not revoke refresh token", ex);
            }
        }
    }
}


﻿using Grooveyard.Domain.Entities;
using Grooveyard.Infrastructure.Data;
using Grooveyard.Infrastructure.Data.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Grooveyard.Infrastructure.Data.Repositories
{
    public class DiscussionRepository : IDiscussionRepository
    {

        private readonly GrooveyardDbContext _context;

        public DiscussionRepository(GrooveyardDbContext context)
        {
            _context = context;

        }
        public async Task<Discussion> CreateDiscussion(Discussion discussion)
        {
            var discussionAdded = _context.Discussions.Add(discussion);
            await _context.SaveChangesAsync();

            return discussionAdded.Entity;
        }

        public async Task<List<Discussion>> GetDiscussions()
        {

            return await _context.Discussions
                .Include(d => d.Genres)
                .OrderByDescending(d => d.UpdatedAt)
                .ToListAsync();
        }

        public async Task<Discussion> GetDiscussion(string discussionId)
        {

            return await _context.Discussions
                .Include(d => d.Genres)
                .FirstOrDefaultAsync(x => x.Id == discussionId);
        }

        public async Task<bool> DeleteDiscussion(Discussion discussion)
        {
            // Fetch all posts related to the discussion
            var posts = await _context.Posts
                .Where(p => p.DiscussionId == discussion.Id)
                .Include(p => p.Comments)
                .ThenInclude(c => c.Likes)
                .ToListAsync();

            // Delete likes associated with comments of each post
            foreach (var post in posts)
            {
                foreach (var comment in post.Comments)
                {
                    _context.Likes.RemoveRange(comment.Likes);
                }

                // Delete the comments themselves
                _context.Comments.RemoveRange(post.Comments);
            }

            // Delete the posts
            _context.Posts.RemoveRange(posts);

            var subscriptions = await _context.Subscriptions.Where(x => x.DiscussionId == discussion.Id).ToListAsync();
            _context.Subscriptions.RemoveRange(subscriptions);


            _context.Discussions.Remove(discussion);

            var saveResult = await _context.SaveChangesAsync();

            return saveResult > 0;
        }


        public async Task<bool> UpdateDiscussionDate(Discussion discussion)
        {
            discussion.UpdatedAt = DateTime.Now;
            var saveResult = await _context.SaveChangesAsync();

            return saveResult > 0;
        }

        public async Task<Genre> GetOrCreateGenre(string genreName)
        {
            Genre genre = null;
            try
            {
                genre = await _context.Genres.FirstOrDefaultAsync(g => g.Name == genreName);
                if (genre == null)
                {
                    genre = new Genre { Name = genreName, Id = Guid.NewGuid().ToString() };
                    _context.Genres.Add(genre);
                    _context.SaveChangesAsync().Wait();
                    genre = _context.Genres.FirstOrDefault(g => g.Name == genreName);
                }
            }

            catch (Exception ex)
            {
                // Log the exception or handle it
            }

            return genre;
        }

        public async Task<List<Genre>> GetAllGenres()
        {
            return await _context.Genres.ToListAsync();
        }

        public async Task<int> GetDiscussionCountForUserAsync(string userId)
        {
            return await _context.Discussions.CountAsync(p => p.UserId == userId);
        }

        public async Task<bool> ToggleSubscription(string discussionId, string userId)
        {
            Subscription existingSubscription = null;
            int saveResult = 0;
            if (!string.IsNullOrEmpty(discussionId))
            {
                existingSubscription = _context.Subscriptions.FirstOrDefault(l => l.UserId == userId && l.DiscussionId == discussionId);
            }

            if (existingSubscription != null)
            {
                _context.Subscriptions.Remove(existingSubscription);
                saveResult = await _context.SaveChangesAsync();
                return saveResult > 0;
            }
            else
            {
                var subscription = new Subscription
                {
                    UserId = userId,
                    DiscussionId = discussionId,
                };

                await _context.Subscriptions.AddAsync(subscription);
                saveResult = await _context.SaveChangesAsync();

                return saveResult > 0;
            }
        }

        public async Task<bool> GetUserSubscription(string discussionId, string userId)
        {
           var existingSubscription = _context.Subscriptions.FirstOrDefault(l => l.UserId == userId && l.DiscussionId == discussionId);

            if (existingSubscription != null)
            {
                return true;
            }
            return false;
        }

        public async Task<int> GetDiscussionSubscriptionCount(string discussionId)
        {
            var existingSubscriptions = _context.Subscriptions.Where(x => x.DiscussionId == discussionId).ToList();

            return existingSubscriptions.Count();
        }

    }
}

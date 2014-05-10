using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SkjaTextar.DAL;
using SkjaTextar.Models;

namespace SkjaTextar.Tests.Mocks 
{
    public class MockTranslationRepository : IUnitOfWork
    {
        private readonly List<Translation> _translations;
        public MockTranslationRepository(List<Translation> translations) 
        {
            _translations = translations;
        }
        public IGenericRepository<Models.Achievement> AchievementRepository
        {
            get { throw new NotImplementedException(); }
        }

        public IGenericRepository<Models.Category> CategoryRepository
        {
            get { throw new NotImplementedException(); }
        }

        public IGenericRepository<Models.Media> MediaRepository
        {
            get { throw new NotImplementedException(); }
        }

        public IGenericRepository<Models.Movie> MovieRepository
        {
            get { throw new NotImplementedException(); }
        }

        public IGenericRepository<Models.Show> ShowRepository
        {
            get { throw new NotImplementedException(); }
        }

        public IGenericRepository<Models.Clip> ClipRepository
        {
            get { throw new NotImplementedException(); }
        }

        public IQueryable<Models.Translation> GetTranslations()
        {
            //get { throw new NotImplementedException(); }
            return _translations.AsQueryable();
        }

        public IGenericRepository<Models.Request> RequestRepository
        {
            get { throw new NotImplementedException(); }
        }

        public IGenericRepository<Models.User> UserRepository
        {
            get { throw new NotImplementedException(); }
        }

        public IGenericRepository<Models.Language> LanguageRepository
        {
            get { throw new NotImplementedException(); }
        }

        public IGenericRepository<Models.Comment> CommentRepository
        {
            get { throw new NotImplementedException(); }
        }

        public IGenericRepository<Models.Report> ReportRepository
        {
            get { throw new NotImplementedException(); }
        }

        public IGenericRepository<Models.RequestVote> RequestVoteRepository
        {
            get { throw new NotImplementedException(); }
        }

        public IGenericRepository<Models.TranslationSegment> TranslationSegmentRepository
        {
            get { throw new NotImplementedException(); }
        }

        public void Save()
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }
}

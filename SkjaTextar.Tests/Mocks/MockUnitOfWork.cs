using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;
using SkjaTextar.DAL;
using SkjaTextar.Models;
using System.Web.Mvc;
using SkjaTextar.ViewModels;
using Microsoft.AspNet.Identity;

// þetta eru tvö skástril
namespace SkjaTextar.Tests.Mocks
{
    public class MockUnitOfWork : IUnitOfWork
    {
        private IGenericRepository<Translation> _translationRepo;

        private IGenericRepository<Achievement> _achievementRepo;
        private IGenericRepository<Category> _categoryRepo;
        private IGenericRepository<Media> _mediaRepo;
        private IGenericRepository<Movie> _movieRepo;
        private IGenericRepository<Show> _showRepo;
        private IGenericRepository<Clip> _clipRepo;
        private IGenericRepository<Request> _requestRepo;
        private IGenericRepository<User> _userRepo;
        private IGenericRepository<Language> _languageRepo;
        private IGenericRepository<Comment> _commentRepo;
        private IGenericRepository<Report> _reportRepo;
        private IGenericRepository<TranslationSegment> _translationSegmentRepo;
        private IGenericRepository<RequestVote> _requestVoteRepo;




        public IGenericRepository<Models.Achievement> AchievementRepository
        {
            get
            {
                if (this._achievementRepo == null)
                {
                    this._achievementRepo = new MockGenericRepository<Achievement>(new List<Achievement>());
                }
                return _achievementRepo;
            }
        }

        public IGenericRepository<Models.Category> CategoryRepository
        {
            get
            {
                if (this._categoryRepo == null)
                {
                    this._categoryRepo = new MockGenericRepository<Category>(new List<Category>());
                }
                return _categoryRepo;
            }
        }

        public IGenericRepository<Models.Media> MediaRepository
        {
            get
            {
                if (this._mediaRepo == null)
                {
                    this._mediaRepo = new MockGenericRepository<Media>(new List<Media>());
                }
                return _mediaRepo;
            }
        }

        public IGenericRepository<Models.Movie> MovieRepository
        {
            get
            {
                if (this._movieRepo == null)
                {
                    this._movieRepo = new MockGenericRepository<Movie>(new List<Movie>());
                }
                return _movieRepo;
            }
        }

        public IGenericRepository<Models.Show> ShowRepository
        {
            get
            {
                if (this._showRepo == null)
                {
                    this._showRepo = new MockGenericRepository<Show>(new List<Show>());
                }
                return _showRepo;
            }
        }

        public IGenericRepository<Models.Clip> ClipRepository
        {
            get
            {
                if (this._clipRepo == null)
                {
                    this._clipRepo = new MockGenericRepository<Clip>(new List<Clip>());
                }
                return _clipRepo;
            }
        }

        public IGenericRepository<Models.Translation> TranslationRepository
        {
            get
            {
                if (this._translationRepo == null)
                {
                    this._translationRepo = new MockGenericRepository<Translation>(new List<Translation>());
                }
                return _translationRepo;
            }
            //get { throw new NotImplementedException(); }

        }

        public IGenericRepository<Models.Request> RequestRepository
        {
            get
            {
                if (this._requestRepo == null)
                {
                    this._requestRepo = new MockGenericRepository<Request>(new List<Request>());
                }
                return _requestRepo;
            }
        }

        public IGenericRepository<Models.User> UserRepository
        {
            get
            {
                if (this._userRepo == null)
                {
                    this._userRepo = new MockGenericRepository<User>(new List<User>());
                }
                return _userRepo;
            }
        }

        public IGenericRepository<Models.Language> LanguageRepository
        {
            get
            {
                if (this._languageRepo == null)
                {
                    this._languageRepo = new MockGenericRepository<Language>(new List<Language>());
                }
                return _languageRepo;
            }
        }

        public IGenericRepository<Models.Comment> CommentRepository
        {
            get
            {
                if (this._commentRepo == null)
                {
                    this._commentRepo = new MockGenericRepository<Comment>(new List<Comment>());
                }
                return _commentRepo;
            }
        }

        public IGenericRepository<Models.Report> ReportRepository
        {
            get
            {
                if (this._reportRepo == null)
                {
                    this._reportRepo = new MockGenericRepository<Report>(new List<Report>());
                }
                return _reportRepo;
            }
        }

        public IGenericRepository<Models.RequestVote> RequestVoteRepository
        {
            get
            {
                if (this._requestVoteRepo == null)
                {
                    this._requestVoteRepo = new MockGenericRepository<RequestVote>(new List<RequestVote>());
                }
                return _requestVoteRepo;
            }
        }

        public IGenericRepository<Models.TranslationSegment> TranslationSegmentRepository
        {
            get
            {
                if (this._translationSegmentRepo == null)
                {
                    this._translationSegmentRepo = new MockGenericRepository<TranslationSegment>(new List<TranslationSegment>());
                }
                return _translationSegmentRepo;
            }
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


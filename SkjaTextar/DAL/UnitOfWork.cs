using System;
using SkjaTextar.Models;

namespace SkjaTextar.DAL
{
    // Code from:
    // http://www.asp.net/mvc/tutorials/getting-started-with-ef-5-using-mvc-4/implementing-the-repository-and-unit-of-work-patterns-in-an-asp-net-mvc-application
    public class UnitOfWork : IUnitOfWork, IDisposable
    {
        private ApplicationDbContext _context = new ApplicationDbContext();
        private IGenericRepository<Achievement> _achievementRepo;
        private IGenericRepository<Category> _categoryRepo;
        private IGenericRepository<Media> _mediaRepo;
        private IGenericRepository<Movie> _movieRepo;
        private IGenericRepository<Show> _showRepo;
        private IGenericRepository<Clip> _clipRepo;
        private IGenericRepository<Translation> _translationRepo;
        private IGenericRepository<Request> _requestRepo;
        private IGenericRepository<User> _userRepo;
        private IGenericRepository<Language> _languageRepo;
        private IGenericRepository<Comment> _commentRepo;

        public IGenericRepository<Achievement> AchievementRepository
        {
            get
            {
                if (this._achievementRepo == null)
                {
                    this._achievementRepo = new GenericRepository<Achievement>(_context);
                }
                return _achievementRepo;
            }
        }

        public IGenericRepository<Category> CategoryRepository
        {
            get
            {
                if (this._categoryRepo == null)
                {
                    this._categoryRepo = new GenericRepository<Category>(_context);
                }
                return _categoryRepo;
            }
        }

        public IGenericRepository<Media> MediaRepository
        {
            get
            {
                if (this._mediaRepo == null)
                {
                    this._mediaRepo = new GenericRepository<Media>(_context);
                }
                return _mediaRepo;
            }
        }

        public IGenericRepository<Movie> MovieRepository
        {
            get
            {
                if (this._movieRepo == null)
                {
                    this._movieRepo = new GenericRepository<Movie>(_context);
                }
                return _movieRepo;
            }
        }

        public IGenericRepository<Show> ShowRepository
        {
            get
            {
                if (this._showRepo == null)
                {
                    this._showRepo = new GenericRepository<Show>(_context);
                }
                return _showRepo;
            }
        }

        public IGenericRepository<Clip> ClipRepository
        {
            get
            {
                if (this._clipRepo == null)
                {
                    this._clipRepo = new GenericRepository<Clip>(_context);
                }
                return _clipRepo;
            }
        }

        public IGenericRepository<Translation> TranslationRepository
        {
            get
            {
                if (this._translationRepo == null)
                {
                    this._translationRepo = new GenericRepository<Translation>(_context);
                }
                return _translationRepo;
            }
        }

        public IGenericRepository<Request> RequestRepository
        {
            get
            {
                if (this._requestRepo == null)
                {
                    this._requestRepo = new GenericRepository<Request>(_context);
                }
                return _requestRepo;
            }
        }

        public IGenericRepository<User> UserRepository
        {
            get
            {
                if(this._userRepo == null)
                {
                    this._userRepo = new GenericRepository<User>(_context);
                }
                return _userRepo;
            }
        }

        public IGenericRepository<Language> LanguageRepository
        {
            get
            {
                if (this._languageRepo == null)
                {
                    this._languageRepo = new GenericRepository<Language>(_context);
                }
                return _languageRepo;
            }
        }

        public IGenericRepository<Comment> CommentRepository
        {
            get
            {
                if (this._commentRepo == null)
                {
                    this._commentRepo = new GenericRepository<Comment>(_context);
                }
                return _commentRepo;
            }
        }

        public void Save()
        {
            _context.SaveChanges();
        }

        private bool _disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!this._disposed)
            {
                if (disposing)
                {
                    _context.Dispose();
                }
            }
            this._disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
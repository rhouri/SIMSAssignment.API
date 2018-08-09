using System.Collections.Generic;
using System.Linq;

namespace SIMSAssignment.API.Services
    {

    // Class to wrap the result of a service call, managing errors returns
    public class ServiceResult
        {
        private bool succeeded = true;
        public virtual bool Succeeded => succeeded;

        private List<string> errors;
        public IEnumerable<string> Errors
            {
            get
                {
                return errors;
                }
            set
                {
                errors = new List<string>(value);
                succeeded = errors?.Any() == false;
                }
            }

        public ServiceResult ()
            {
            }

        public ServiceResult ( IEnumerable<string> errors )
            {
            Errors = new List<string>(errors);
            }

        public ServiceResult ( string error ) : this(new[] { error })
            {
            }

        public void AddErrors ( IEnumerable<string> errors )
            {
            if (this.errors == null)
                this.errors = new List<string>();
            this.errors.AddRange(errors);
            succeeded = false;
            }
        }

    // A generic class for returning service results 
    // this will include a resulting object together with a succeed/fail flag and possible error messages

    public class ServiceResult<T> :ServiceResult where T : class
        {
        public ServiceResult ()
            {
            }

        public ServiceResult ( T result )
            {
            Result = result;
            }

        public ServiceResult ( IEnumerable<string> errors ) : base(errors) { }

        public ServiceResult ( string error ) : base(error) { }

        public T Result
            {
            get; set;
            }

        //public Pager Pager { get; private set; }

        public override bool Succeeded => Result != null && base.Succeeded;
        }
    }

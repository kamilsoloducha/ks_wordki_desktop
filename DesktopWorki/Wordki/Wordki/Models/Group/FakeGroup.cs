using WordkiModel;
using System;
using System.Collections.Generic;

namespace Wordki.Models
{
    public class FakeGroup : IGroup
    {

        private static IGroup _instance;
        private static object _lock = new object();

        public static IGroup Group
        {
            get
            {
                lock (_lock)
                {
                    if (_instance == null)
                    {
                        _instance = new FakeGroup();
                    }
                }
                return _instance;
            }
        }

        private FakeGroup()
        {

        }

        public long Id
        {
            get
            {
                return -1;
            }

            set { }
        }

        public LanguageType Language1
        {
            get
            {
                return LanguageType.Default;
            }

            set { }
        }

        public LanguageType Language2
        {
            get
            {
                return LanguageType.Default;
            }

            set { }
        }

        public string Name
        {
            get
            {
                return string.Empty;
            }

            set { }
        }

        public IList<IResult> Results
        {
            get
            {
                return new IResult[0];
            }

            set { }
        }

        public int State
        {
            get
            {
                return 0;
            }

            set { }
        }

        public IList<IWord> Words
        {
            get
            {
                return new IWord[0];
            }

            set
            {
            }
        }

        public DateTime CreationDate
        {
            get
            {
                return new DateTime();
            }

            set
            {
            }
        }

        public void AddResult(IResult result)
        {
        }

        public void AddWord(IWord word)
        {
        }
    }
}

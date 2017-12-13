using Repository.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using Wordki.Helpers.WordComparer;

namespace Wordki.Models.Lesson
{

    [Serializable]
    public abstract class Lesson
    {
        public IList<IResult> ResultList { get; protected set; }
        public Queue<IWord> WordQueue { get; protected set; }
        public IList<IWord> BeginWordsList { get; protected set; }
        public IWord SelectedWord { get; protected set; }
        public bool IsCorrect { get; protected set; }
        public bool IsChecked { get; protected set; }
        public int CurrentDrawer { get; protected set; }
        public Util.Timer Timer { get; private set; }
        public int Counter { get; set; }
        public IWordComparer WordComparer { get; set; }
        public ILessonSettings LessonSettings { get; set; }

        protected Lesson()
        {
            BeginWordsList = new List<IWord>();
            WordQueue = new Queue<IWord>();
            ResultList = new List<IResult>();
            Timer = new Util.Timer();
            Counter = 1;
            IsCorrect = false;
        }

        protected abstract void CreateWordList(IEnumerable<IWord> words);
        protected abstract void CreateResultList();
        public abstract void Known();
        public abstract void Check(string translation);

        public virtual void InitLesson(IEnumerable<IWord> words)
        {
            CreateWordList(words);
            CreateResultList();
        }

        /// <summary>
        /// Wybranie kolejnego slowa
        /// </summary>
        public virtual void NextWord()
        {
            SelectedWord = (WordQueue.Count > 0) ? WordQueue.Dequeue() : null;
            IsChecked = false;
            if (SelectedWord != null)
                CurrentDrawer = SelectedWord.Drawer;
        }

        /// <summary>
        /// Zaznaczenie slowa jako nieznane
        /// </summary>
        public virtual void Unknown()
        {
            if (Counter++ <= BeginWordsList.Count)
            {
                SelectedWord.Drawer = 0; //reset szuflady
                IResult lResult = ResultList.FirstOrDefault(x => x.Group.Id == SelectedWord.Group.Id);
                if (lResult != null)
                {
                    lResult.Wrong++;
                }
            }
            //przesuwamy SelectedWord na poczatek listy
            WordQueue.Enqueue(SelectedWord);
            NextWord();
        }

        protected virtual bool CheckTranslation(string pOriginalWord, string pTranslationWord)
        {
            return WordComparer.IsEqual(pOriginalWord, pTranslationWord);
        }

        public virtual void FinishLesson()
        {
            foreach (IResult lResult in ResultList)
            {
                lResult.TimeCount = (short)(Timer.GetTime() / ResultList.Count);
            }
        }

        public int GetCorrect()
        {
            return ResultList.Sum(lResult => lResult.Correct);
        }

        public int GetAccepted()
        {
            return ResultList.Sum(lResult => lResult.Accepted);
        }

        public int GetWrong()
        {
            return ResultList.Sum(lResult => lResult.Wrong);
        }

        public virtual int GetMaxResult()
        {
            return GetWrong() + GetAccepted() + GetCorrect();
        }

        public virtual List<int> GetDrawerCount()
        {
            List<int> lDrawerCountList = new List<int>();
            for (int i = 0; i < 5; i++)
            {
                lDrawerCountList.Add(0);
            }
            foreach (IWord lWord in BeginWordsList)
            {
                lDrawerCountList[lWord.Drawer]++;
            }
            return lDrawerCountList;
        }

        public void DeleteSelectedWord()
        {
            BeginWordsList.Remove(SelectedWord);
        }

        public virtual int[] GetDrawerValues()
        {
            int[] lTempValues = new int[5];
            foreach (IWord lWord in BeginWordsList)
            {
                lTempValues[lWord.Drawer]++;
            }
            return lTempValues;
        }

        public virtual double GetProgress()
        {
            int remain = WordQueue.Count + (SelectedWord == null ? 0 : 1);
            return (BeginWordsList.Count - remain) * 100d / BeginWordsList.Count;
        }
    }
}

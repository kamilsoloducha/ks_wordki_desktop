using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Repository.Models.Enums;
using Wordki.Helpers;
using Repository.Models;

namespace Wordki.Models.Lesson
{
    [Serializable]
    public class IntensiveLesson : TypingLesson
    {
        private List<Queue<IWord>> DrawersList { get; set; }
        private const int Drawers = 5;
        private const int DrawerCount = 6;

        public IntensiveLesson() : base()
        {
            DrawersList = new List<Queue<IWord>>(Drawers);
            for (int i = 0; i < Drawers; i++)
            {
                DrawersList.Add(new Queue<IWord>());
            }
            CurrentDrawer = -1;
        }

        public override void NextWord()
        {
            Stopwatch stopWatch = Stopwatch.StartNew();
            //PrintDrawers();
            IsChecked = false;
            SelectedWord = WordList.Count == 0 ? NextWordWithEmptyList() : NextWordWithContainedList();
            Counter = BeginWordsList.Count - WordList.Count;
            stopWatch.Stop();
            Console.WriteLine("E - NextWord - {0}", stopWatch.ElapsedTicks);
        }

        public override void Known()
        {
            Stopwatch stopWatch = Stopwatch.StartNew();
            MakeKnwon();
            Dequeue();
            if (CurrentDrawer < Drawers - 1)
                DrawersList[CurrentDrawer + 1].Enqueue(SelectedWord);
            NextWord();
            stopWatch.Stop();
            Console.WriteLine("E - Knwon - {0}", stopWatch.ElapsedTicks);
        }

        public override void Check(string translation)
        {
            IsChecked = true;
            switch (LessonSettings.TranslationDirection)
            {
                case TranslationDirection.FromSecond: IsCorrect = translation.Trim().Equals(SelectedWord.Language1); break;
                case TranslationDirection.FromFirst: IsCorrect = translation.Trim().Equals(SelectedWord.Language2); break;
            }
        }

        public override void Unknown()
        {
            Stopwatch stopWatch = Stopwatch.StartNew();
            MakeUnknwon();
            Dequeue();
            DrawersList[0].Enqueue(SelectedWord);
            NextWord();
            stopWatch.Stop();
            Console.WriteLine("E - Unknwon - {0}", stopWatch.ElapsedTicks);
        }

        private IWord NextWordWithEmptyList()
        {
            Stopwatch stopWatch = Stopwatch.StartNew();
            CurrentDrawer = 4;
            for (int i = 0; i < DrawersList.Count - 1; i++)
            {
                if (DrawersList[i].Count > 0 && DrawersList[i + 1].Count < DrawerCount)
                {
                    CurrentDrawer = i;
                }
            }
            stopWatch.Stop();
            Console.WriteLine("E - NextWordWithEmptyList - {0}", stopWatch.ElapsedTicks);
            return DrawersList[CurrentDrawer].Count > 0 ? DrawersList[CurrentDrawer].Peek() : null;
        }

        private IWord NextWordWithContainedList()
        {
            Stopwatch stopWatch = Stopwatch.StartNew();
            int fullList = -1;
            for (int i = 0; i < DrawersList.Count; i++)
            {
                if (DrawersList[i].Count >= DrawerCount)
                {
                    fullList = i;
                }
            }
            CurrentDrawer = fullList;
            stopWatch.Stop();
            Console.WriteLine("E - NextWordWithContainedList - {0}", stopWatch.ElapsedTicks);
            return fullList < 0 ? WordList.Peek() : DrawersList[fullList].Peek();
        }

        private void Dequeue()
        {
            if (CurrentDrawer < 0)
            {
                WordList.Dequeue();
            }
            else
            {
                DrawersList[CurrentDrawer].Dequeue();
            }
        }

        private void MakeKnwon()
        {
            IResult result = ResultList.FirstOrDefault(x => x.Group.Id == SelectedWord.Group.Id);
            if (result == null)
            {
                return;
            }
            if (IsCorrect)
            {
                result.Correct++;
            }
            else
            {
                result.Accepted++;
            }
        }

        private void MakeUnknwon()
        {
            IResult result = ResultList.FirstOrDefault(x => x.Group.Id == SelectedWord.Group.Id);
            if (result == null)
            {
                return;
            }
            result.Wrong++;
        }

        public override int[] GetDrawerValues()
        {
            int[] lTempValues = new int[5];
            for (int i = 0; i < Drawers; i++)
            {
                lTempValues[i] = DrawersList[i].Count;
            }
            return lTempValues;
        }

        public override double GetProgress()
        {
            int drawersCount = DrawersList.Sum(x => x.Count);
            return (BeginWordsList.Count - (WordList.Count + drawersCount)) * 100d / BeginWordsList.Count;
        }

        private void PrintDrawers()
        {
            Stopwatch stopWatch = Stopwatch.StartNew();
            LoggerSingleton.LogInfo("WordList:");
            foreach (Word word in WordList)
            {
                LoggerSingleton.LogInfo("Słowo: {0} | {1}", word.Language1, word.Language2);
            }
            LoggerSingleton.LogInfo("DrawersList:");
            for (int i = 0; i < Drawers; i++)
            {
                LoggerSingleton.LogInfo("szuflada: {0} | słów: {1}", i, DrawersList[i].Count);
                foreach (Word word in DrawersList[i])
                {
                    LoggerSingleton.LogInfo("Słowo: {0} | {1}", word.Language1, word.Language2);
                }
            }
            stopWatch.Stop();
            Console.WriteLine("E - PrintDrawers - {0}", stopWatch.ElapsedTicks);
        }
    }
}

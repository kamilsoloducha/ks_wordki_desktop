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
            IsChecked = false;
            SelectedWord = WordQueue.Count == 0 ? NextWordWithEmptyList() : NextWordWithContainedList();
            Counter = BeginWordsList.Count - WordQueue.Count;
        }

        public override void Known()
        {
            MakeKnwon();
            Dequeue();
            if (CurrentDrawer < Drawers - 1)
                DrawersList[CurrentDrawer + 1].Enqueue(SelectedWord);
            NextWord();
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
            MakeUnknwon();
            Dequeue();
            DrawersList[0].Enqueue(SelectedWord);
            NextWord();
        }

        private IWord NextWordWithEmptyList()
        {
            CurrentDrawer = 4;
            for (int i = 0; i < DrawersList.Count - 1; i++)
            {
                if (DrawersList[i].Count > 0 && DrawersList[i + 1].Count < DrawerCount)
                {
                    CurrentDrawer = i;
                }
            }
            return DrawersList[CurrentDrawer].Count > 0 ? DrawersList[CurrentDrawer].Peek() : null;
        }

        private IWord NextWordWithContainedList()
        {
            int fullList = -1;
            for (int i = 0; i < DrawersList.Count; i++)
            {
                if (DrawersList[i].Count >= DrawerCount)
                {
                    fullList = i;
                }
            }
            CurrentDrawer = fullList;
            return fullList < 0 ? WordQueue.Peek() : DrawersList[fullList].Peek();
        }

        private void Dequeue()
        {
            if (CurrentDrawer < 0)
            {
                WordQueue.Dequeue();
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
            return (BeginWordsList.Count - (WordQueue.Count + drawersCount)) * 100d / BeginWordsList.Count;
        }
    }
}

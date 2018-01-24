using Oazachaosu.Core.Common;
using System.Collections.Generic;
using Wordki.Models;
using WordkiModel;


namespace Repository.Model.DTOConverters
{
    public static class WordConverter
    {

        public static WordDTO GetDTOFromModel(IWord word)
        {
            return new WordDTO()
            {
                Id = word.Id,
                GroupId = word.Group.Id,
                Language1 = word.Language1,
                Language2 = word.Language2,
                Language1Comment = word.Language1Comment,
                Language2Comment = word.Language2Comment,
                Drawer = word.Drawer,
                IsVisible = word.IsVisible,
                State = word.State,
                IsSelected = word.IsSelected,
                RepeatingCounter = word.RepeatingCounter,
                Comment = word.Comment,
                LastRepeating = word.LastRepeating,
            };
        }

        public static IWord GetModelFromDTO(WordDTO word)
        {
            return new Word()
            {
                Id = word.Id,
                Group = new Group() { Id = word.GroupId },
                Language1 = word.Language1,
                Language2 = word.Language2,
                Language1Comment = word.Language1Comment,
                Language2Comment = word.Language2Comment,
                Drawer = word.Drawer,
                IsVisible = word.IsVisible,
                State = word.State,
                IsSelected = word.IsSelected,
                RepeatingCounter = word.RepeatingCounter,
                Comment = word.Comment,
                LastRepeating = word.LastRepeating,
            };
        }

        public static IEnumerable<IWord> GetWordsFromDTOs(IEnumerable<WordDTO> words)
        {
            foreach(WordDTO word in words)
            {
                yield return GetModelFromDTO(word);
            }
        }

        public static IEnumerable<WordDTO> GetDTOsFromWords(IEnumerable<IWord> words)
        {
            foreach (IWord word in words)
            {
                yield return GetDTOFromModel(word);
            }
        }
    }
}

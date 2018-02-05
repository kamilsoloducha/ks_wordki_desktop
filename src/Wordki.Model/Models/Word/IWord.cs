using System.ComponentModel;
using System;

namespace WordkiModel
{
    public interface IWord : ICloneable
    {
        [PropertyIndex(0)]
        long Id { get; set; }

        [PropertyIndex(2)]
        IGroup Group { get; set; }

        [PropertyIndex(3)]
        string Language1 { get; set; }

        [PropertyIndex(4)]
        string Language2 { get; set; }

        [PropertyIndex(5)]
        string Language1Comment { get; set; }

        [PropertyIndex(6)]
        string Language2Comment { get; set; }

        [PropertyIndex(7)]
        byte Drawer { get; set; }

        [DefaultValue(true)]
        [PropertyIndex(8)]
        bool IsVisible { get; set; }

        [PropertyIndex(9)]
        int State { get; set; }

        [DefaultValue(false)]
        [PropertyIndex(10)]
        bool IsSelected { get; set; }

        [DefaultValue(0)]
        [PropertyIndex(11)]
        ushort RepeatingCounter { get; set; }

        [PropertyIndex(12)]
        DateTime LastRepeating { get; set; }

        [PropertyIndex(13)]
        string Comment { get; set; }
    }
}
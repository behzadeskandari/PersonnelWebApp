﻿using System.ComponentModel;

namespace Personnel.Api.Components.Enums
{
    public enum PersianMonth : byte
    {
        [Description("فروردین")]
        Farvardin = 1,
        [Description("اردیبهشت")]
        Ordibehesht = 2,
        [Description("خرداد")]
        Khordad = 3,
        [Description("تیر")]
        Tir = 4,
        [Description("مرداد")]
        Mordad = 5,
        [Description("شهریور")]
        Shahrivar = 6,
        [Description("مهر")]
        Mehr = 7,
        [Description("آبان")]
        Aban = 8,
        [Description("آذر")]
        Azar = 9,
        [Description("دی")]
        Dey = 10,
        [Description("بهمن")]
        Bahman = 11,
        [Description("اسفند")]
        Esfand = 12

    }
}

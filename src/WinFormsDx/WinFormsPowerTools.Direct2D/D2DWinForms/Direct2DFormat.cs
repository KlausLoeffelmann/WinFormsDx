using System.Drawing;
using System.Globalization;
using Windows.Win32;
using Windows.Win32.Graphics.DirectWrite;

namespace System.Windows.Forms.Direct2D
{
    internal class Direct2DFormat
    {
        private static readonly QueuedLookup<Direct2DFormat> s_cached2DFormats = new(100);
        // private static readonly int MaxFormatCache = 100;

        private readonly IDWriteFactory _writeFactory;
        private IDWriteTextFormat? _cachedInstance;

        private string _fontFamilyName;
        private float _fontSize;
        private float? _lineSpacing;
        private float? _baseLine;
        private readonly char _delimiterChar = Char.MinValue;
        private readonly int _delimiterCount = 1;

        private DWRITE_FONT_WEIGHT _dwFontWeight = DWRITE_FONT_WEIGHT.DWRITE_FONT_WEIGHT_NORMAL;
        private DWRITE_FONT_STYLE _dwFontStyle = DWRITE_FONT_STYLE.DWRITE_FONT_STYLE_NORMAL;
        private DWRITE_FONT_STRETCH _dwFontStretch = DWRITE_FONT_STRETCH.DWRITE_FONT_STRETCH_NORMAL;
        private DWRITE_LINE_SPACING_METHOD _dwLineSpacingMethod = DWRITE_LINE_SPACING_METHOD.DWRITE_LINE_SPACING_METHOD_DEFAULT;
        private DWRITE_PARAGRAPH_ALIGNMENT _dwParagraphAlignment = DWRITE_PARAGRAPH_ALIGNMENT.DWRITE_PARAGRAPH_ALIGNMENT_NEAR;
        private DWRITE_TEXT_ALIGNMENT _dwTextAlignment = DWRITE_TEXT_ALIGNMENT.DWRITE_TEXT_ALIGNMENT_LEADING;
        private DWRITE_TRIMMING_GRANULARITY _dwTrimmingGranularity = DWRITE_TRIMMING_GRANULARITY.DWRITE_TRIMMING_GRANULARITY_NONE;
        private DWRITE_WORD_WRAPPING _dwWordWrapping = DWRITE_WORD_WRAPPING.DWRITE_WORD_WRAPPING_NO_WRAP;

        public Direct2DFormat(
            IDWriteFactory writeFactory,
            string fontFamilyName,
            float fontSize)
        {
            _writeFactory = writeFactory;
            _fontFamilyName = fontFamilyName;
            _fontSize = fontSize;
        }

        internal IDWriteTextFormat GetInstance()
        {
            if (_cachedInstance is null)
            {
                var textFormat = CreateTextFormat(
                    _writeFactory,
                    _fontFamilyName,
                    _fontSize,
                    _dwFontWeight,
                    _dwFontStyle,
                    _dwFontStretch);

                textFormat.SetParagraphAlignment(_dwParagraphAlignment);
                textFormat.SetTextAlignment(_dwTextAlignment);

                var trimming = new DWRITE_TRIMMING
                {
                    granularity = _dwTrimmingGranularity
                };

                if (_delimiterChar != char.MinValue)
                {
                    trimming.delimiter = Convert.ToUInt32(_delimiterChar);
                    trimming.delimiterCount = (uint)_delimiterCount;
                }

                textFormat.SetTrimming(trimming, null);
                textFormat.SetWordWrapping(_dwWordWrapping);

                if (_lineSpacing.HasValue)
                {
                    textFormat.SetLineSpacing(
                        _dwLineSpacingMethod,
                        _lineSpacing.Value,
                        _baseLine is null ? (_fontSize + 2) * 8 / 10 : _baseLine.Value);
                }

                _cachedInstance = textFormat;
            }

            return _cachedInstance;
        }

        public string FontFamilyName
        {
            get => _fontFamilyName;
            set
            {
                _fontFamilyName = value;
                _cachedInstance = null;
            }
        }

        public float FontSize
        {
            get => _fontSize;
            set
            {
                _fontSize = value;
                _cachedInstance = null;
            }
        }

        public float? LineSpacing
        {
            get => _lineSpacing;
            set
            {
                _lineSpacing = value;
                _cachedInstance = null;
            }
        }

        public float? BaseLine
        {
            get => _baseLine;
            set
            {
                _baseLine = value;
                _cachedInstance = null;
            }
        }

        public IDirectWriteTextFormat.FontWeight FontWeight
        {
            get => (IDirectWriteTextFormat.FontWeight)_dwFontWeight;
            set
            {
                _dwFontWeight = (DWRITE_FONT_WEIGHT)value;
                _cachedInstance = null;
            }
        }

        public IDirectWriteTextFormat.FontStyle FontStyle
        {
            get => (IDirectWriteTextFormat.FontStyle)_dwFontStyle;
            set
            {
                _dwFontStyle = (DWRITE_FONT_STYLE)value;
                _cachedInstance = null;
            }
        }

        public IDirectWriteTextFormat.FontStretch FontStretch
        {
            get => (IDirectWriteTextFormat.FontStretch)_dwFontStretch;
            set
            {
                _dwFontStretch = (DWRITE_FONT_STRETCH)value;
                _cachedInstance = null;
            }
        }

        public IDirectWriteTextFormat.LineSpacingMethod LineSpacingMethod
        {
            get => (IDirectWriteTextFormat.LineSpacingMethod)_dwLineSpacingMethod;
            set
            {
                _dwLineSpacingMethod = (DWRITE_LINE_SPACING_METHOD)value;
                _cachedInstance = null;
            }
        }

        public IDirectWriteTextFormat.ParagraphAlignment ParagraphAlignment
        {
            get => (IDirectWriteTextFormat.ParagraphAlignment)_dwParagraphAlignment;
            set
            {
                _dwParagraphAlignment = (DWRITE_PARAGRAPH_ALIGNMENT)value;
                _cachedInstance = null;
            }
        }

        public IDirectWriteTextFormat.TextAlignment TextAlignment
        {
            get => (IDirectWriteTextFormat.TextAlignment)_dwTextAlignment;
            set
            {
                _dwTextAlignment = (DWRITE_TEXT_ALIGNMENT)value;
                _cachedInstance = null;
            }
        }

        public IDirectWriteTextFormat.TrimmingGranularity TrimmingGranularity
        {
            get => (IDirectWriteTextFormat.TrimmingGranularity)_dwTrimmingGranularity;
            set
            {
                _dwTrimmingGranularity = (DWRITE_TRIMMING_GRANULARITY)value;
                _cachedInstance = null;
            }
        }

        public IDirectWriteTextFormat.WordWrapping WordWrapping
        {
            get => (IDirectWriteTextFormat.WordWrapping)_dwWordWrapping;
            set
            {
                _dwWordWrapping = (DWRITE_WORD_WRAPPING)value;
                _cachedInstance = null;
            }
        }

        public static Direct2DFormat FromFont(Font font, IDWriteFactory writeFactory)
        {
            if (TryGetFromFont(font, out var d2dFormat))
            {
                return d2dFormat;
            }

            d2dFormat = new Direct2DFormat(
                writeFactory,
                font.FontFamily.Name,
                font.Size)
            {
                FontWeight = font.Bold
                ? IDirectWriteTextFormat.FontWeight.Bold
                : IDirectWriteTextFormat.FontWeight.Normal,

                FontStyle = font.Italic
                ? IDirectWriteTextFormat.FontStyle.Italic
                : IDirectWriteTextFormat.FontStyle.Normal
            };

            s_cached2DFormats.Add(font, d2dFormat);

            return d2dFormat;
        }

        private static bool TryGetFromFont(Font font, out Direct2DFormat d2dFormat)
        {
            return s_cached2DFormats.TryGetValue(font, out d2dFormat!);
        }

        public static Direct2DFormat FromFontAndStringFormat(
            Font font,
            StringFormat stringFormat,
            IDWriteFactory writeFactory)
        {
            if (TryGetFromFontAndStringFormat(font, stringFormat, out var d2dFormat))
            {
                return d2dFormat;
            }

            d2dFormat = FromFont(font, writeFactory);

            d2dFormat.TextAlignment = stringFormat.Alignment switch
            {
                StringAlignment.Near => IDirectWriteTextFormat.TextAlignment.Leading,
                StringAlignment.Far => IDirectWriteTextFormat.TextAlignment.Trailing,
                StringAlignment.Center => IDirectWriteTextFormat.TextAlignment.Center,

                _ => throw new NotImplementedException($"Text alignment '{stringFormat.Alignment}' is not supported.")
            };

            d2dFormat.ParagraphAlignment = stringFormat.LineAlignment switch
            {
                StringAlignment.Near => IDirectWriteTextFormat.ParagraphAlignment.Near,
                StringAlignment.Far => IDirectWriteTextFormat.ParagraphAlignment.Far,
                StringAlignment.Center => IDirectWriteTextFormat.ParagraphAlignment.Center,

                _ => throw new NotImplementedException($"Text alignment '{stringFormat.LineAlignment}' is not supported.")
            };

            d2dFormat.TrimmingGranularity = stringFormat.Trimming switch
            {
                StringTrimming.None => IDirectWriteTextFormat.TrimmingGranularity.None,
                StringTrimming.EllipsisCharacter => IDirectWriteTextFormat.TrimmingGranularity.Character,
                StringTrimming.EllipsisWord => IDirectWriteTextFormat.TrimmingGranularity.Word,

                _ => throw new NotImplementedException($"Text alignment '{stringFormat.Trimming}' is not supported.")
            };

            s_cached2DFormats.Add(font, stringFormat, d2dFormat);

            return d2dFormat;
        }

        private static bool TryGetFromFontAndStringFormat(Font font, StringFormat stringFormat, out Direct2DFormat d2dFormat)
        {
            return s_cached2DFormats.TryGetValue(HashCode.Combine(font, stringFormat), out d2dFormat!);
        }

        private static IDWriteTextFormat CreateTextFormat(
            IDWriteFactory writeFactory,
            string fontFamilyName,
            float fontSize,
            DWRITE_FONT_WEIGHT fontWeight = DWRITE_FONT_WEIGHT.DWRITE_FONT_WEIGHT_NORMAL,
            DWRITE_FONT_STYLE fontStyle = DWRITE_FONT_STYLE.DWRITE_FONT_STYLE_NORMAL,
            DWRITE_FONT_STRETCH fontStretch = DWRITE_FONT_STRETCH.DWRITE_FONT_STRETCH_NORMAL,
            string? localName = null)
        {
            if (string.IsNullOrEmpty(localName))
            {
                localName = CultureInfo.CurrentCulture.Name;
            }

            writeFactory.CreateTextFormat(
                fontFamilyName,
                fontCollection: null,
                fontWeight,
                fontStyle,
                fontStretch,
                fontSize,
                localName,
                out var textFormat);

            return textFormat;
        }
    }
}

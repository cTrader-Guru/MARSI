/*  CTRADER GURU --> Template 1.0.6

    Homepage    : https://ctrader.guru/
    Telegram    : https://t.me/ctraderguru
    Twitter     : https://twitter.com/cTraderGURU/
    Facebook    : https://www.facebook.com/ctrader.guru/
    YouTube     : https://www.youtube.com/channel/UCKkgbw09Fifj65W5t5lHeCQ
    GitHub      : https://github.com/cTraderGURU/
    TOS         : https://ctrader.guru/termini-del-servizio/

*/

using cAlgo.API;
using cAlgo.API.Indicators;

namespace cAlgo
{

    [Levels(20, 30, 70, 80)]
    [Indicator(IsOverlay = false, TimeZone = TimeZones.UTC, ScalePrecision = 1, AccessRights = AccessRights.None)]
    public class MARSI : Indicator
    {

        #region Enums

        // --> Eventuali enumeratori li mettiamo qui

        #endregion

        #region Identity

        /// <summary>
        /// Nome del prodotto, identificativo, da modificare con il nome della propria creazione
        /// </summary>
        public const string NAME = "MARSI";

        /// <summary>
        /// La versione del prodotto, progressivo, utilie per controllare gli aggiornamenti se viene reso disponibile sul sito ctrader.guru
        /// </summary>
        public const string VERSION = "1.0.5";

        #endregion

        #region Params

        /// <summary>
        /// Identità del prodotto nel contesto di ctrader.guru
        /// </summary>
        [Parameter(NAME + " " + VERSION, Group = "Identity", DefaultValue = "https://ctrader.guru/product/marsi/")]
        public string ProductInfo { get; set; }

        [Parameter("MA Period", Group = "MA", DefaultValue = 20)]
        public int MAPeriods { get; set; }

        [Parameter("MA Type", Group = "MA", DefaultValue = MovingAverageType.Exponential)]
        public MovingAverageType MaType { get; set; }

        [Parameter("MA Source", Group = "MA")]
        public DataSeries Source { get; set; }

        [Parameter("RSI Period", Group = "RSI", DefaultValue = 20)]
        public int RSIPeriods { get; set; }

        [Parameter("Fast Period", Group = "RSI", DefaultValue = 10)]
        public int TPeriods { get; set; }

        [Output("RSI", LineColor = "Red")]
        public IndicatorDataSeries Rsi { get; set; }

        [Output("Slow", LineColor = "DodgerBlue")]
        public IndicatorDataSeries Trigger { get; set; }

        #endregion

        #region Property

        private RelativeStrengthIndex _rsi;
        private MovingAverage _ma;
        private ExponentialMovingAverage _ema;

        #endregion

        #region Indicator Events

        /// <summary>
        /// Viene generato all'avvio dell'indicatore, si inizializza l'indicatore
        /// </summary>
        protected override void Initialize()
        {

            // --> Stampo nei log la versione corrente
            Print("{0} : {1}", NAME, VERSION);

            _ma = Indicators.MovingAverage(Source, MAPeriods, MaType);
            _rsi = Indicators.RelativeStrengthIndex(_ma.Result, RSIPeriods);
            _ema = Indicators.ExponentialMovingAverage(_rsi.Result, TPeriods);

        }

        /// <summary>
        /// Generato ad ogni tick, vengono effettuati i calcoli dell'indicatore
        /// </summary>
        /// <param name="index">L'indice della candela in elaborazione</param>
        public override void Calculate(int index)
        {

            Rsi[index] = _rsi.Result[index];
            Trigger[index] = _ema.Result[index];

        }

        #endregion


        #region Private Methods


        #endregion

    }

}

using System;

namespace Domena
{
    public static class Helper
    {
        public static MiaraKata StopnieDoMiaryKatowej(double stopnie)
        {
            double minuty_azymutu = (stopnie - Math.Floor(stopnie)) * 60.0;
            double sekundy_azymutu = (minuty_azymutu - Math.Floor(minuty_azymutu)) * 60.0;
            double dziesiatki_azymutu = (sekundy_azymutu - Math.Floor(sekundy_azymutu)) * 10.0;
            //pozbycie się części ułamkowej

            var rezultat = new MiaraKata();
            rezultat.minuty_azymutu = Math.Floor(minuty_azymutu);
            rezultat.sekundy_azymutu = Math.Floor(sekundy_azymutu);
            rezultat.dziesiatki_azymutu = Math.Floor(dziesiatki_azymutu);

            return rezultat;
        }
    }
}
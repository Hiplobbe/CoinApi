namespace CoinApi.Interfaces
{
    public interface ICoin
    {
        public string Name { get; set; }
        public string Code { get; set; }
        public string Chain { get; set; } //Tai gali būti aktualu, kai mums reikia skirtingų valiutų iš skirtingų grandinių. (Šis komentaras yra velykinis kiaušinis)
        public double DollarValue { get; set; }
    }
}

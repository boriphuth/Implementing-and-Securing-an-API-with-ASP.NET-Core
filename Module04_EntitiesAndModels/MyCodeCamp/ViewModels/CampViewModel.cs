using System;

namespace MyCodeCamp.ViewModels
{
    public class CampViewModel
    {
        #region Camp Common
        public int Id { get; set; }
        public string Moniker { get; set; }
        public string Name { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int Length { get; set; }
        public string Description { get; set; }
        #endregion

        #region Location Data
        public string LocationAddress1 { get; set; }
        public string LocationAddress2 { get; set; }
        public string LocationAddress3 { get; set; }
        public string LocationCityTown { get; set; }
        public string LocationStateProvince { get; set; }
        public string LocationPostalCode { get; set; }
        public string LocationCountry { get; set; }
        #endregion
    }
}

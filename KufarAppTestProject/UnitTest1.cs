using KufarAppProject.ApiClasses;

namespace KufarAppTestProject
{
    public class UnitTest1
    {
        [Fact]
        public void GetFlatsInCoords_CheckingNormalCoords_GetFlats()
        {
            //Arrange
            var expected = 0;
            var kufarApi = new KufarApi();
            List<(double, double)> points = [(53.95900934394375, 27.388272055808265), (53.97322744376776, 27.715553698310803), 
                (53.822537452296004, 27.713288239041795), (53.845860777571495, 27.377390837733667)];

            //Act
            var result = kufarApi.GetFlatsInCoordinates(points);

            //Assert
            Assert.NotEqual(expected, result.Count);
        }

        [Fact]
        public void GetFlatsInCoords_NoFigureCoords_RaiseException()
        {
            //Arrange
            var kufarApi = new KufarApi();
            List<(double, double)> points = [(53.95900934394375, 27.388272055808265), (53.97322744376776, 27.715553698310803)];

            //Act and Assert
            Assert.Throws<ArgumentException>(() => kufarApi.GetFlatsInCoordinates(points));
        }

        [Fact]
        public void GetFlatsForBooking_StandartParameters_GetRequiredDistrict()
        {
            //Arrange
            var kufarApi = new KufarApi();
            var district = "окт€брьский";

            //Act
            var result = kufarApi.GetFlatsForBooking(district);

            //Assert
            Assert.All(result, element => Assert.Contains(district, element.Item2, StringComparison.OrdinalIgnoreCase));
        }

        [Fact]
        public void GetFlatsForBookingByDate_StandartParameters_GetRequiredDistrict()
        {
            //Arrange
            var kufarApi = new KufarApi();
            var district = "фрунзенский";
            var date = new DateOnly(2024, 6, 1);

            //Act
            var result = kufarApi.GetFlatsForBookingByDate(district, date);

            //Assert
            Assert.All(result, element => Assert.Equal(date, element.Item2));
        }
    }
}
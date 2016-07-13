using Xunit;
using System.Collections.Generic;
using System;
using System.Data;
using System.Data.SqlClient;

namespace RestaurantsList
{
  public class RestaurantTest: IDisposable
  {
    public RestaurantTest()
    {
      DBConfiguration.ConnectionString = "Data Source=(localdb)\\mssqllocaldb;Initial Catalog=restaurants_test;Integrated Security=SSPI;";
    }

    [Fact]
    public void Test_RestaurantsEmptyAtFirst()
    {
      //Arrange, Act
      int result = Restaurant.GetAll().Count;

      //Assert
      Assert.Equal(0, result);
    }

    [Fact]
    public void Test_Equal_ReturnsTrueForSameName()
    {
      //Arrange, Act
      Restaurant firstRestaurant = new Restaurant("Ox",1);
      Restaurant secondRestaurant = new Restaurant("Ox",1);

      //Assert
      Assert.Equal(firstRestaurant, secondRestaurant);
    }

    [Fact]
    public void Test_Save()
    {
      Restaurant testRestaurant = new Restaurant("Ox",1);
      testRestaurant.Save();

      //Act
      List<Restaurant> result = Restaurant.GetAll();
      List<Restaurant> testList = new List<Restaurant>{testRestaurant};

      //Assert
      Assert.Equal(testList, result);
    }
    public void Dispose()
    {
      Restaurant.DeleteAll();
      Cuisine.DeleteAll();
    }
  }
}

using Xunit;
using System.Collections.Generic;
using System;
using System.Data;
using System.Data.SqlClient;

namespace RestaurantsList
{
  public class CuisineTest : IDisposable
  {
    public CuisineTest()
    {
      DBConfiguration.ConnectionString = "Data Source=(localdb)\\mssqllocaldb;Initial Catalog=Restaurants_test;Integrated Security=SSPI;";
    }

    [Fact]
    public void Test_CategoriesEmptyAtFirst()
    {
      //Arrange, Act
      int result = Cuisine.GetAll().Count;

      //Assert
      Assert.Equal(0, result);
    }

    [Fact]
    public void Test_Equal_ReturnsTrueForSameName()
    {
      //Arrange, Act
      Cuisine firstCuisine = new Cuisine("Household chores");
      Cuisine secondCuisine = new Cuisine("Household chores");

      //Assert
      Assert.Equal(firstCuisine, secondCuisine);
    }

    [Fact]
    public void Test_Save_SavesCuisineToDatabase()
    {
      //Arrange
      Cuisine testCuisine = new Cuisine("Household chores");
      testCuisine.Save();

      //Act
      List<Cuisine> result = Cuisine.GetAll();
      List<Cuisine> testList = new List<Cuisine>{testCuisine};

      //Assert
      Assert.Equal(testList, result);
    }

    [Fact]
    public void Test_Save_AssignsIdToCuisineObject()
    {
      //Arrange
      Cuisine testCuisine = new Cuisine("Household chores");
      testCuisine.Save();

      //Act
      Cuisine savedCuisine = Cuisine.GetAll()[0];

      int result = savedCuisine.GetId();
      int testId = testCuisine.GetId();

      //Assert
      Assert.Equal(testId, result);
    }

    [Fact]
    public void Test_Find_FindsCuisineInDatabase()
    {
      //Arrange
      Cuisine testCuisine = new Cuisine("Household chores");
      testCuisine.Save();

      //Act
      Cuisine foundCuisine = Cuisine.Find(testCuisine.GetId());

      //Assert
      Assert.Equal(testCuisine, foundCuisine);
    }

    [Fact]
    public void Test_GetRestaurants_RetrievesAllRestaurantsWithCuisine()
    {
      //Arrange
      Cuisine testCuisine = new Cuisine("Household chores");
      testCuisine.Save();

      //Act
      Restaurant firstRestaurant = new Restaurant ("Mow the lawn", testCuisine.GetId());
      firstRestaurant.Save();
      Restaurant secondRestaurant = new Restaurant("Do the dishes", testCuisine.GetId());
      secondRestaurant.Save();

      List<Restaurant> testRestaurantList = new List<Restaurant> {firstRestaurant, secondRestaurant};
      List<Restaurant> resultRestaurantList = testCuisine.GetRestaurants();

      //Assert
      Assert.Equal(testRestaurantList, resultRestaurantList);
      }

      [Fact]

      public void Test_Update_UpdatesCuisineInDatabase()
      {
        //Arrange
        string name = "Home stuff";
        Cuisine testCuisine = new Cuisine(name);
        testCuisine.Save();
        string newName = "Work stuff";

        //Act
        testCuisine.Update(newName);

        string result = testCuisine.GetCuisine();

        //Assert
        Assert.Equal(newName, result);
      }
    public void Dispose()
    {
      Restaurant.DeleteAll();
      Cuisine.DeleteAll();
    }
  }
}

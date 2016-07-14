using System.Collections.Generic;
using System.Data.SqlClient;
using System;

namespace RestaurantsList
{
  public class Restaurant
  {
    private int _id;
    private string _name;
    private int _cuisineId;

    public Restaurant(string Name, int CuisineId, int Id = 0)
    {
      _id = Id;
      _name = Name;
      _cuisineId = CuisineId;
    }

    public override bool Equals(System.Object otherRestaurant)
    {
      if(!(otherRestaurant is Restaurant))
      {
        return false;
      }
      else
      {
        Restaurant newRestaurant = (Restaurant) otherRestaurant;
        bool idEquality = this.GetId() == newRestaurant.GetId();
        bool nameEquality = this.GetName() == newRestaurant.GetName();
        bool cuisineIdEquality = this.GetCuisineId() == newRestaurant.GetCuisineId();
        return (idEquality && nameEquality && cuisineIdEquality);
      }
    }
    public int GetId()
    {
      return _id;
    }
    public void SetId(int newId)
    {
      _id = newId;
    }
    public string GetName()
    {
      return _name;
    }
    public void SetName(string newName)
    {
      _name = newName;
    }


    public int GetCuisineId()
    {
      return _cuisineId;
    }
    public void SetCuisineId(int newCuisineId)
    {
      _cuisineId = newCuisineId;
    }

    public static List<Restaurant> GetAll()
    {
      List<Restaurant> AllRestaurants = new List<Restaurant>{};

      SqlConnection conn = DB.Connection();
      SqlDataReader rdr = null;
      conn.Open();

      SqlCommand cmd = new SqlCommand("SELECT * FROM restaurant_name;", conn);
      rdr = cmd.ExecuteReader();

      while(rdr.Read())
      {
        int restaurantId = rdr.GetInt32(0);
        string restaurantName = rdr.GetString(1);
        int restaurantCuisineId = rdr.GetInt32(2);
        Restaurant newRestaurant = new Restaurant(restaurantName, restaurantCuisineId, restaurantId);
        AllRestaurants.Add(newRestaurant);
      }
      if (rdr != null)
      {
        rdr.Close();
      }
      if (conn != null)
      {
        conn.Close();
      }
      return AllRestaurants;
    }


    public List<Review> GetReviews()
     {
       SqlConnection conn = DB.Connection();
       SqlDataReader rdr = null;
       conn.Open();

       SqlCommand cmd = new SqlCommand("SELECT * FROM reviews WHERE restaurant_id = @RestaurantId;", conn);
       SqlParameter restaurantIdParameter = new SqlParameter();
       restaurantIdParameter.ParameterName = "@RestaurantId";
       restaurantIdParameter.Value = this.GetId();

       cmd.Parameters.Add(restaurantIdParameter);
       rdr = cmd.ExecuteReader();

       List<Review> restaurants = new List<Review> {};
       while(rdr.Read())
       {
         int restaurantId = rdr.GetInt32(0);
         string restaurantName = rdr.GetString(1);
         int restaurantRestaurantId = rdr.GetInt32(2);

         Review newReview = new Review(restaurantName, restaurantRestaurantId, restaurantId);
         restaurants.Add(newReview);
       }
       if (rdr != null)
       {
         rdr.Close();
       }
       if (conn != null)
       {
         conn.Close();
       }
       return restaurants;
     }

     public static Restaurant Find(int id)
     {
       SqlConnection conn = DB.Connection();
       SqlDataReader rdr = null;
       conn.Open();

       SqlCommand cmd = new SqlCommand("SELECT * FROM restaurant_name WHERE id = @RestaurantId;", conn);
       SqlParameter cuisineIdParameter = new SqlParameter();
       cuisineIdParameter.ParameterName = "@RestaurantId";
       cuisineIdParameter.Value = id.ToString();

       cmd.Parameters.Add(cuisineIdParameter);
       rdr = cmd.ExecuteReader();

       int foundRestaurantId = 0;
       string foundRestaurantName = null;
       int foundId = 0;

       while(rdr.Read())
       {
         foundRestaurantId = rdr.GetInt32(0);

         foundRestaurantName = rdr.GetString(1);
         foundId = rdr.GetInt32(2);
       }
       Restaurant foundRestaurant = new Restaurant(foundRestaurantName, foundId, foundRestaurantId);

       if (rdr != null)
       {
         rdr.Close();
       }
       if (conn != null)
       {
         conn.Close();
       }
       return foundRestaurant;
     }

    public static Restaurant FindByName(string name)
    {
      SqlConnection conn = DB.Connection();
      SqlDataReader rdr = null;
      conn.Open();

      SqlCommand cmd = new SqlCommand("SELECT * FROM cuisine WHERE cuisine_name = @RestaurantName;", conn);
      SqlParameter cuisineIdParameter = new SqlParameter();
      cuisineIdParameter.ParameterName = "@RestaurantName";
      cuisineIdParameter.Value = name;
      cmd.Parameters.Add(cuisineIdParameter);
      rdr = cmd.ExecuteReader();

      int foundRestaurantId = 0;
      string foundRestaurantName = null;

      while(rdr.Read())
      {
        foundRestaurantId = rdr.GetInt32(0);
        foundRestaurantName = rdr.GetString(1);
      }
      Restaurant foundRestaurant = new Restaurant(foundRestaurantName, foundRestaurantId);

      if (rdr != null)
      {
        rdr.Close();
      }
      if (conn != null)
      {
        conn.Close();
      }
      return foundRestaurant;
    }

    public void Save()
    {
      SqlConnection conn = DB.Connection();
      SqlDataReader rdr;
      conn.Open();

      SqlCommand cmd = new SqlCommand("INSERT INTO restaurant_name (name, cuisine_id) OUTPUT INSERTED.id VALUES (@RestaurantName, @RestaurantCuisineId);", conn);

      SqlParameter nameParameter = new SqlParameter();
      nameParameter.ParameterName = "@RestaurantName";
      nameParameter.Value = this.GetName();

      SqlParameter cuisineIdParameter = new SqlParameter();
      cuisineIdParameter.ParameterName = "@RestaurantCuisineId";
      cuisineIdParameter.Value = this.GetCuisineId();

      cmd.Parameters.Add(nameParameter);
      cmd.Parameters.Add(cuisineIdParameter);

      rdr = cmd.ExecuteReader();

      while(rdr.Read())
      {
        this._id = rdr.GetInt32(0);
      }
      if (rdr != null)
      {
        rdr.Close();
      }
      if (conn != null)
      {
        conn.Close();
      }
    }
    public static void DeleteAll()
    {
      SqlConnection conn = DB.Connection();
      conn.Open();
      SqlCommand cmd = new SqlCommand("DELETE FROM restaurant_name;", conn);
      cmd.ExecuteNonQuery();
    }

  }
}

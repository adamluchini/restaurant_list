using System.Collections.Generic;
using System.Data.SqlClient;
using System;

namespace RestaurantsList
{
  public class Cuisine
  {
    private int _id;
    private string _cuisine;

    public Cuisine(string Cuisine, int Id = 0)
    {
      _id = Id;
      _cuisine = Cuisine;
    }

    public override bool Equals(System.Object otherCuisine)
    {
      if(!(otherCuisine is Cuisine))
      {
        return false;
      }
      else
      {
        Cuisine newCuisine = (Cuisine) otherCuisine;
        bool idEquality = this.GetId() == newCuisine.GetId();
        bool cuisineEquality = this.GetCuisine() == newCuisine.GetCuisine();
        return (idEquality && cuisineEquality);
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
    public string GetCuisine()
    {
      return _cuisine;
    }
    public void SetCuisine(string newCuisine)
    {
      _cuisine = newCuisine;
    }

    public static List<Cuisine> GetAll()
    {
      List<Cuisine> AllCuisines = new List<Cuisine>{};

      SqlConnection conn = DB.Connection();
      SqlDataReader rdr = null;
      conn.Open();

      SqlCommand cmd = new SqlCommand("SELECT * FROM cuisine", conn);
      rdr = cmd.ExecuteReader();

      while(rdr.Read())
      {
        int restaurantId = rdr.GetInt32(0);
        string restaurantCuisine = rdr.GetString(1);
        Cuisine newCuisine = new Cuisine(restaurantCuisine, restaurantId);
        AllCuisines.Add(newCuisine);
      }
      if (rdr != null)
      {
        rdr.Close();
      }
      if (conn != null)
      {
        conn.Close();
      }
      return AllCuisines;
    }

    public List<Restaurant> GetRestaurants()
     {
       SqlConnection conn = DB.Connection();
       SqlDataReader rdr = null;
       conn.Open();

       SqlCommand cmd = new SqlCommand("SELECT * FROM restaurant_name WHERE cuisine_id = @CuisineId;", conn);
       SqlParameter cuisineIdParameter = new SqlParameter();
       cuisineIdParameter.ParameterName = "@CuisineId";
       cuisineIdParameter.Value = this.GetId();
       cmd.Parameters.Add(cuisineIdParameter);
       rdr = cmd.ExecuteReader();

       List<Restaurant> restaurants = new List<Restaurant> {};
       while(rdr.Read())
       {
         int restaurantId = rdr.GetInt32(0);
         string restaurantName = rdr.GetString(1);
         int restaurantCuisineId = rdr.GetInt32(2);
         Restaurant newRestaurant = new Restaurant(restaurantName, restaurantCuisineId, restaurantId);
         restaurants.Add(newRestaurant);
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

    public void Save()
    {
      SqlConnection conn = DB.Connection();
      SqlDataReader rdr;
      conn.Open();

      SqlCommand cmd = new SqlCommand("INSERT INTO cuisine (cuisine_name) OUTPUT INSERTED.id VALUES (@RestaurantCuisine);", conn);

      SqlParameter cuisineParameter = new SqlParameter();
      cuisineParameter.ParameterName = "@RestaurantCuisine";
      cuisineParameter.Value = this.GetCuisine();

      cmd.Parameters.Add(cuisineParameter);

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

    public static Cuisine Find(int id)
    {
      SqlConnection conn = DB.Connection();
      SqlDataReader rdr = null;
      conn.Open();

      SqlCommand cmd = new SqlCommand("SELECT * FROM cuisine WHERE id = @CuisineId;", conn);
      SqlParameter cuisineIdParameter = new SqlParameter();
      cuisineIdParameter.ParameterName = "@CuisineId";
      cuisineIdParameter.Value = id.ToString();
      cmd.Parameters.Add(cuisineIdParameter);
      rdr = cmd.ExecuteReader();

      int foundCuisineId = 0;
      string foundCuisineName = null;

      while(rdr.Read())
      {
        foundCuisineId = rdr.GetInt32(0);
        foundCuisineName = rdr.GetString(1);
      }
      Cuisine foundCuisine = new Cuisine(foundCuisineName, foundCuisineId);

      if (rdr != null)
      {
        rdr.Close();
      }
      if (conn != null)
      {
        conn.Close();
      }
      return foundCuisine;
    }

    public void Update(string newName)
    {
      SqlConnection conn = DB.Connection();
      SqlDataReader rdr;
      conn.Open();

      SqlCommand cmd = new SqlCommand("UPDATE cuisine SET cuisine_name = @NewName OUTPUT INSERTED.cuisine_name WHERE id = @CuisineIdId;", conn);

      SqlParameter newNameParameter = new SqlParameter();
      newNameParameter.ParameterName = "@NewName";
      newNameParameter.Value = newName;
      cmd.Parameters.Add(newNameParameter);


      SqlParameter cuisineIdParameter = new SqlParameter();
      cuisineIdParameter.ParameterName = "@CuisineIdId";
      cuisineIdParameter.Value = this.GetId();
      cmd.Parameters.Add(cuisineIdParameter);
      rdr = cmd.ExecuteReader();

      while(rdr.Read())
      {
        this._cuisine = rdr.GetString(0);
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
      SqlCommand cmd = new SqlCommand("DELETE FROM cuisine;", conn);
      cmd.ExecuteNonQuery();
    }

  }
}

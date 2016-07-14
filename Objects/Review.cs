using System.Collections.Generic;
using System.Data.SqlClient;
using System;

namespace RestaurantsList
{
  public class Review
  {
    private int _id;
    private string _description;
    private int _restaurantId;

    public Review(string Description, int RestaurantId, int Id = 0)
    {
      _id = Id;
      _description = Description;
      _restaurantId = RestaurantId;
    }

    public override bool Equals(System.Object otherReview)
    {
      if(!(otherReview is Review))
      {
        return false;
      }
      else
      {
        Review newReview = (Review) otherReview;
        bool idEquality = this.GetId() == newReview.GetId();
        bool descriptionEquality = this.GetDescription() == newReview.GetDescription();
        bool restaurantIdEquality = this.GetRestaurantId() == newReview.GetRestaurantId();
        return (idEquality && descriptionEquality && restaurantIdEquality);
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
    public string GetDescription()
    {
      return _description;
    }
    public void SetDescription(string newDescription)
    {
      _description = newDescription;
    }


    public int GetRestaurantId()
    {
      return _restaurantId;
    }
    public void SetRestaurantId(int newRestaurantId)
    {
      _restaurantId = newRestaurantId;
    }

    public static List<Review> GetAll()
    {
      List<Review> AllReviews = new List<Review>{};

      SqlConnection conn = DB.Connection();
      SqlDataReader rdr = null;
      conn.Open();

      SqlCommand cmd = new SqlCommand("SELECT * FROM reviews;", conn);
      rdr = cmd.ExecuteReader();

      while(rdr.Read())
      {
        int restaurantId = rdr.GetInt32(0);
        string restaurantDescription = rdr.GetString(1);
        int restaurantRestaurantId = rdr.GetInt32(2);
        Review newReview = new Review(restaurantDescription, restaurantRestaurantId, restaurantId);
        AllReviews.Add(newReview);
      }
      if (rdr != null)
      {
        rdr.Close();
      }
      if (conn != null)
      {
        conn.Close();
      }
      return AllReviews;
    }


    public void Save()
    {
      SqlConnection conn = DB.Connection();
      SqlDataReader rdr;
      conn.Open();

      SqlCommand cmd = new SqlCommand("INSERT INTO reviews (description, restaurant_id) OUTPUT INSERTED.id VALUES (@ReviewDescription, @ReviewRestaurantId);", conn);

      SqlParameter descriptionParameter = new SqlParameter();
      descriptionParameter.ParameterName = "@ReviewDescription";
      descriptionParameter.Value = this.GetDescription();

      SqlParameter restaurantIdParameter = new SqlParameter();
      restaurantIdParameter.ParameterName = "@ReviewRestaurantId";
      restaurantIdParameter.Value = this.GetRestaurantId();

      cmd.Parameters.Add(descriptionParameter);
      cmd.Parameters.Add(restaurantIdParameter);

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
      SqlCommand cmd = new SqlCommand("DELETE FROM reviews;", conn);
      cmd.ExecuteNonQuery();
    }

  }
}

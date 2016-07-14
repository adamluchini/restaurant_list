using System.Collections.Generic;
using Nancy;
using Nancy.ViewEngines.Razor;
using System;

namespace RestaurantsList
{
  public class HomeModule : NancyModule
  {
    public HomeModule()
    {
      Get["/"]= _ => {
        List<Cuisine> AllCuisines = Cuisine.GetAll();
        return View["index.cshtml", AllCuisines];
      };

      Get["/restaurants"] = _ => {
        List<Restaurant> AllRestaurants = Restaurant.GetAll();
        return View["restaurants.cshtml", AllRestaurants];
      };

      Get["/cuisines"] = _ => {
        List<Cuisine> AllCuisines = Cuisine.GetAll();
        return View["cuisines.cshtml", AllCuisines];
      };

      Get["/cuisines/new"] = _ => {
        return View["cuisines_form.cshtml"];
      };

      Post["/cuisines/new"] = _ => {
        Cuisine newCuisine = new Cuisine(Request.Form["cuisine-name"]);
        newCuisine.Save();
        return View["success.cshtml"];
      };

      Get["/restaurants/new"] = _ => {
        List<Cuisine> AllCuisines = Cuisine.GetAll();
        return View["restaurants_form.cshtml", AllCuisines];
      };

      Post["/restaurants/new"] = _ => {
        Restaurant newRestaurant = new Restaurant(Request.Form["restaurant-name"], Request.Form["cuisine-id"]);
        newRestaurant.Save();
        return View["success.cshtml"];
      };

      Get["/reviews/new"] = _ => {
        List<Restaurant> AllRestaurants = Restaurant.GetAll();
        return View["reviews_form.cshtml", AllRestaurants];
      };

      Post["/reviews/new"] = _ => {
        Review newReview = new Review(Request.Form["review-name"], Request.Form["cuisine-id"]);
        newReview.Save();
        return View["success.cshtml"];
      };

      Post["/restaurants/delete"] = _ => {
        Restaurant.DeleteAll();
        return View["restaurants_cleared.cshtml"];
      };

      Post["/cuisines/delete"] = _ =>{
        Cuisine.DeleteAll();
        return View["cuisines_cleared.cshtml"];
      };

      Get["/cuisines/{id}"] = parameters => {
        Dictionary<string, object> model = new Dictionary<string, object>();
        var SelectedCuisine = Cuisine.Find(parameters.id);
        var CuisineRestaurants = SelectedCuisine.GetRestaurants();
        model.Add("cuisine", SelectedCuisine);
        model.Add("restaurants", CuisineRestaurants);
        return View["cuisine.cshtml", model];
      };

      Get["/restaurants/{id}"] = parameters => {
        // Dictionary<string, object> model = new Dictionary<string, object>();
        Restaurant SelectedRestaurant = Restaurant.Find(parameters.id);
        //Console.WriteLine(SelectedRestaurant.GetRestaurantId() + " ");
        // var RestaurantReviews = SelectedRestaurant.GetReviews();
        // model.Add("Restaurant", SelectedRestaurant);
        // model.Add("reviews", RestaurantReviews);
        return View["reviews.cshtml", SelectedRestaurant.GetReviews()];
      };

      Get["/search"]= parameters => {
        return View["search.cshtml"];
      };

      Post["/result"]= _ => {
        Cuisine selectedCuisine = Cuisine.FindByName(Request.Form["search"]);
        return View["result.cshtml",selectedCuisine.GetRestaurants()];
      };
    }
  }
}

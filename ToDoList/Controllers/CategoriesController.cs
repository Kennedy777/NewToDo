using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using ToDoList.Models;

namespace ToDoList.Controllers
{
  public class CategoriesController : Controller
  {
    [HttpGet("/categories")]
    public ActionResult Index()
    {
        List<Category> allCategories = Category.GetAll();
        return View(allCategories);
    }
    [HttpPost("/categories")]
    public ActionResult Create(string categoryName)
    {
      Category newCategory = new Category(categoryName);
      List<Category> allCategories = Category.GetAll();
      return View("Index", allCategories);
    }
    [HttpGet("/categories/new")]
    public ActionResult New()
    {
        return View();
    }
    // This one creates new Items within a given Category, not new Categories:
    [HttpPost("/categories/{categoryId}/items")]
    public ActionResult Create(int categoryId, string itemDescription, DateTime due)
    {
      Dictionary<string, object> model = new Dictionary<string, object>();
      Category foundCategory = Category.Find(categoryId);
      Item newItem = new Item(itemDescription, due);
      newItem.Save();
      foundCategory.AddItem(newItem);
      List<Item> categoryItems = foundCategory.GetItems();
      model.Add("items", categoryItems);
      model.Add("category", foundCategory);
      return View("Show", model);
    }
    [HttpGet("/categories/{id}")]
    public ActionResult Show(int id)
    {
        Dictionary<string, object> model = new Dictionary<string, object>();
        Category selectedCategory = Category.Find(id);
        List<Item> categoryItems = selectedCategory.GetItems();
        model.Add("category", selectedCategory);
        model.Add("items", categoryItems);
        return View(model);
    }
    [HttpPost("/categories/delete")]
    public ActionResult Delete()
    {
        Category.ClearAll();
        return View();
    }

    // [HttpGet("/categories/{categoryId}/itemssort")]
    // public ActionResult Sort(int categoryId, string itemDescription, DateTime dueDate  )
    // {
    //   Dictionary<string, object> model = new Dictionary<string, object>();
    //   Category foundCategory = Category.Find(categoryId);
    //   // Item newItem = new Item(itemDescription, dueDate);
    //   // newItem.Save();
    //   // foundCategory.AddItem(newItem);
    //   List<Item> sortedItems = Item.Sort();
    //   model.Add("items", sortedItems);
    //   model.Add("category", foundCategory);
    //   return View("Show", model);
    // }
  }
}

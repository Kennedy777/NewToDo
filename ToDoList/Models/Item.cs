using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;
using System.Globalization;

namespace ToDoList.Models
{
  public class Item
  {
      private string _description;
      private DateTime _due;
      private int _id;

      public Item (string description, DateTime due, int id = 0)
      {
          _description = description;
          _id = id;
          _due = due;
      }
      public string GetDescription()
      {
          return _description;
      }
      public int GetId()
      {
        return 0;
      }
      public DateTime GetDueDate()
      {
        return _due;
      }

      public void SetDescription(string newDescription)
      {
        _description = newDescription;
      }

      public void SetDueDate(DateTime due)
      {
        _due = due;
      }

      public static List<Item> GetAll()
      {
          List<Item> allItems = new List<Item> {};
          MySqlConnection conn = DB.Connection();
          conn.Open();
          MySqlCommand cmd = conn.CreateCommand() as MySqlCommand;
          cmd.CommandText = @"SELECT * FROM items;";
          MySqlDataReader rdr = cmd.ExecuteReader() as MySqlDataReader;
          while(rdr.Read())
          {
              int id = rdr.GetInt32(0);
              string description = rdr.GetString(1);
              DateTime due = rdr.GetDateTime(2);
              Item item = new Item(description, due, id);
              allItems.Add(item);
          }
          conn.Close();
          if (conn != null)
          {
              conn.Dispose();
          }
          return allItems;
      }
      public static void ClearAll()
      {
        MySqlConnection conn = DB.Connection();
        conn.Open();
        var cmd = conn.CreateCommand() as MySqlCommand;
        cmd.CommandText = @"DELETE FROM items;";
        cmd.ExecuteNonQuery();
        conn.Close();
        if (conn != null)
        {
          conn.Dispose();
        }
      }
      public override bool Equals(System.Object otherItem)
      {
        if (!(otherItem is Item))
        {
          return false;
        }
        else
        {
          Item item = (Item) otherItem;
          bool idEquality = (this.GetId() == item.GetId());
          bool descriptionEquality = (this.GetDescription() == item.GetDescription());
          return (idEquality && descriptionEquality);
        }
      }
      public static Item Find(int itemId)
      {
        MySqlConnection conn = DB.Connection();
        conn.Open();
        var cmd = conn.CreateCommand() as MySqlCommand;
        cmd.CommandText = @"SELECT * FROM `items` WHERE itemId = @thisId;";
        MySqlParameter thisId = new MySqlParameter();
        thisId.ParameterName = "@thisId";
        thisId.Value = itemId;
        cmd.Parameters.Add(thisId);
        var rdr = cmd.ExecuteReader() as MySqlDataReader;
        int id = 0;
        string description = "";
        DateTime due = new DateTime();
        while (rdr.Read())
        {
          id = rdr.GetInt32(0);
          description = rdr.GetString(1);
          due = rdr.GetDateTime(2);
        }
        Item foundItem = new Item(description, due, id);

        conn.Close();
        if (conn != null)
        {
          conn.Dispose();
        }
        return foundItem;
      }
      public void Save()
      {
        MySqlConnection conn = DB.Connection();
        conn.Open();
        var cmd = conn.CreateCommand() as MySqlCommand;
        cmd.CommandText = @"INSERT INTO items (description, due) VALUES (@ItemDescription, @due);";
        MySqlParameter description = new MySqlParameter();
        description.ParameterName = "@ItemDescription";
        description.Value = this._description;
        MySqlParameter due = new MySqlParameter();
        due.ParameterName = "@due";
        due.Value = this._due;
        cmd.Parameters.Add(description);
        cmd.Parameters.Add(due);
        cmd.ExecuteNonQuery();
        _id = (int) cmd.LastInsertedId;

        conn.Close();
        if (conn != null)
        {
          conn.Dispose();
        }
      }


  }
}

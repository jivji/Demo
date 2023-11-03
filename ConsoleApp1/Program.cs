// See https://aka.ms/new-console-template for more information

using System;
using System.Data.SqlClient;
using DataAccess;
using DataAccess.Objects;

Console.WriteLine("Hello, World!");

var connection = new SqlConnection(Configuration.GetConnectionString());

var childDao = new ChildDao(connection);

// Create a child
var newChild = new Child
{
  Name = "john12",
  Description = "john@example.com"
};
//childDao.Add(newChild);
//
// // Retrieve a user by ID
// int childId = 1;
// var child = childDao.Get(childId);
//
// if (child != null)
// {
//   Console.WriteLine($"Child get by ID: {child.Id}, Name: {child.Name}, Description: {child.Description}");
// }
//
// // Retrieve all users
// var allChildren = childDao.GetAll();
// foreach (var u in allChildren)
// {
//   Console.WriteLine($"Child ID: {u.Id}, Name: {u.Name}, Description: {u.Description}");
// }
//
// var grandParentDao = new GrandParentDao(connection);
//
// // Create a child
// var newGP = new GrandParent()
// {
//   Name = "one1",
//   Description = "john@example.com",
//   PrimaryChild = 13
// };
// //grandParentDao.Add(newGP);
//
// // Retrieve a user by ID
// int gpId = 1;
// var gp = grandParentDao.Get(gpId);
//
// if (gp != null)
// {
//   Console.WriteLine($"GrandParent get by ID: {gp.Id}, Name: {gp.Name}, Description: {gp.Description}");
// }
//
// // Retrieve all grand parents
// var allGp = grandParentDao.GetAll();
// foreach (var u in allGp)
// {
//   Console.WriteLine($"GrandParent ID: {u.Id}, Name: {u.Name}, Description: {u.Description}, PrimaryChild: {u.PrimaryChild}");
// }


var parentDao = new ParentDao(connection);

// Create a child
var newParent = new Parent()
{
  Id = 1,
  Name = "one16",
  Description = "john@example.com"
};
parentDao.UpdateParent(newParent);

//parentDao.AddChildToParent(5, 17);

var children = new int[]{14,15};
//parentDao.AddChildrenToParent(7, children);

var updateParent = new Parent()
{
  Id = 1,
  Name = "one12",
  Description = "john@example.com"
};
parentDao.UpdateParent(updateParent);


parentDao.AddChildrenToParent(12, new[] {17, 19});

parentDao.DeleteParent(5);

// Retrieve a user by ID
int pId = 1;
var p = parentDao.GetParent(pId);

if (p != null)
{
  Console.WriteLine($"Parent get by ID: {p.Id}, Name: {p.Name}, Description: {p.Description}");
}

// Retrieve all parents
var allP = parentDao.GetAllParents();
foreach (var u in allP)
{
  Console.WriteLine($"Parent ID: {u.Id}, Name: {u.Name}, Description: {u.Description}");
}




This application is a demo providing a brief introduction to its functionality. It includes three projects and an SQL database extract in the file db.sql.

The database consists of three main tables:

GrandParents, each of which must have a primary child. GrandParents can have one or many Parents.
Parents, which can have one or many Children. Additionally, one child can be a child to many Parents.
There is a many-to-many relationship between Parents and Children.
The fields in the tables are simple, such as Name and Description. Two constraints are defined in the tables: Duplicate names are not allowed, and it is not permitted to add a non-existing item to the database.

The DataAccess project is defined using Dapper as a micro ORM (Object-Relational Mapper) for .NET, because of it's lightweight and efficient way to perform data access in C#. Dapper is known for its excellent performance, resulting in faster execution compared to some other ORMs. I haven't used so far so wanted to experience it's strenghts. I liked that dapper directly executes SQL queries and maps the results to objects without a lot of additional overhead, making it a good choice for scenarios where raw performance is crucial. It doesn't have a lot of abstractions or complex configurations,and can be advantageous for small to medium-sized projects or when you want more control over your SQL queries.

APIApplication is choosen so can showcase how different systems, services, or applications can communicate and exchange data in the scenario. It demonstrate how data can be fetched from a database or an external source. This is relevant to showcase the separation of concerns between the data layer and the presentation layer. On top of this I plan to to integrate with my own frontend project that will allow me to simulate such integrations for the demo. 

APIApplicationTest because APIs are easily testable, which makes them suitable for demonstrating integration testing scenarios.

Remark: This is not a fully polished application, and it serves as a basis for further discussions.


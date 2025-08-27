using AzureCosmosDBApplication;
using Bogus;
using Microsoft.Azure.Cosmos;
using Database = Microsoft.Azure.Cosmos.Database;

const string connection = "cosmosdb-example-string-connection";

var sixMonthsAgo = DateTime.UtcNow.AddMonths(-6).ToString("yyyy-MM");
short studentGenerated = 100;
var faker = new Faker<Student>()
    .RuleFor(s => s.id, f => Guid.NewGuid().ToString())
    .RuleFor(s => s.FirstName, f => f.Name.FirstName())
    .RuleFor(s => s.LastName, f => f.Name.LastName())
    .RuleFor(s => s.Email, (f, s) => f.Internet.Email(s.FirstName, s.LastName))
    .RuleFor(s => s.EnrollmentDateYearMonth,
        f => f.Date.Between(new DateTime(2020, 1, 1, 0, 0, 0, DateTimeKind.Utc), DateTime.Now).ToString("yyyy-MM"));


// Create a new instance of the Cosmos Client
var options = new CosmosClientOptions { AllowBulkExecution = true };
var client = new CosmosClient(connection, options);


// Read account information
var account = await client.ReadAccountAsync();
Console.WriteLine($"Account Id: {account.Id}");
Console.WriteLine($"Account Readable Regions: {account.ReadableRegions.FirstOrDefault()?.Name}");


// Create database studentdb if not exists
Database database = await client.CreateDatabaseIfNotExistsAsync("studentdb");
Console.WriteLine($"Database Id: {database.Id}");


// Create container Students if not exists with partition key /EnrollmentDate
Container container = await database.CreateContainerIfNotExistsAsync("Students", "/EnrollmentDateYearMonth");
Console.WriteLine($"Container Id: {container.Id}");


// Create Operation = Generate and insert 100 students if there is no student in the container
var resultSet = await container
    .GetItemQueryIterator<int>(new QueryDefinition("SELECT VALUE COUNT(1) FROM c"))
    .ReadNextAsync();

while (resultSet.FirstOrDefault() <= 0 && studentGenerated > 0)
{
    var student = faker.Generate();

    await container.CreateItemAsync(student, new PartitionKey(student.EnrollmentDateYearMonth));

    Console.WriteLine($"Created student {student.id}");

    studentGenerated--;
}

Console.WriteLine("Student Container has 100 students");

// Read Operation = Read all students in the container
const string sql = "SELECT * FROM Student AS s ORDER BY s.FirstName";

var query = new QueryDefinition(sql);

using var feedIterator = container.GetItemQueryIterator<Student>(query);
while (feedIterator.HasMoreResults)
{
    var response = await feedIterator.ReadNextAsync();

    foreach (var student in response)
        Console.WriteLine($"Read student {student.id} - {student.FirstName} {student.LastName}");
}


// Read Operation = Query students enrolled in the last 6 months
var queryDefinition = new QueryDefinition("SELECT * FROM c WHERE c.EnrollmentDateYearMonth >= @sixMonthsAgo")
    .WithParameter("@sixMonthsAgo", sixMonthsAgo);

var queryResultSetIterator = container.GetItemQueryIterator<Student>(queryDefinition);

var students = new List<Student>();

while (queryResultSetIterator.HasMoreResults)
{
    var response = await queryResultSetIterator.ReadNextAsync();
    students.AddRange(response);
}

Console.WriteLine($"Students enrolled in the last 6 months: {students.Count}");

foreach (var student in students)
    Console.WriteLine($"Name: {student.FirstName} {student.LastName}");


// Batch Multiple Operations = Generate two students with the same partition key
var randomEnrollmentDateYearMonth = DateTime.UtcNow.AddMonths(-new Random().Next(1, 12)).ToString("yyyy-MM");

var fakerBatch = new Faker<Student>()
    .RuleFor(s => s.id, f => Guid.NewGuid().ToString())
    .RuleFor(s => s.FirstName, f => f.Name.FirstName())
    .RuleFor(s => s.LastName, f => f.Name.LastName())
    .RuleFor(s => s.Email, (f, s) => f.Internet.Email(s.FirstName, s.LastName))
    .RuleFor(s => s.EnrollmentDateYearMonth, randomEnrollmentDateYearMonth);

var student1 = fakerBatch.Generate();
var student2 = fakerBatch.Generate();

Console.WriteLine($"Transaction Batch Operation for Partition: {randomEnrollmentDateYearMonth}");

var batch = container.CreateTransactionalBatch(new PartitionKey(randomEnrollmentDateYearMonth))
    .CreateItem(student1)
    .CreateItem(student2);

var batchResponse = await batch.ExecuteAsync();

Console.WriteLine($"Batch operation completed with status code: {batchResponse.StatusCode}");
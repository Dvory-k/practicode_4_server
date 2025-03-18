// using Microsoft.AspNetCore.Mvc;
// using Microsoft.EntityFrameworkCore;
// using Microsoft.OpenApi.Models;
// using TodoApi;

// var builder = WebApplication.CreateBuilder(args);

// builder.Services.AddControllers();
// builder.Services.AddEndpointsApiExplorer();
// builder.Services.AddSwaggerGen(c=>{
// c.SwaggerDoc("v1",new OpenApiInfo{
//     Version="v1"
// });
// });

// builder.Services.AddDbContext<ToDoDbContext>(options =>
//     options.UseMySql(builder.Configuration.GetConnectionString("ToDoDB"), new MySqlServerVersion(new Version(8, 0, 41))));

// builder.Services.AddCors(option => option.AddPolicy("AllowAll",
//     p => p.AllowAnyOrigin()
//     .AllowAnyMethod()
//     .AllowAnyHeader()));

// var app = builder.Build();

// app.UseCors("AllowAll");

// app.UseSwagger();
//     app.UseSwagger();
//    if (builder.Environment.IsDevelopment())
// { 
//     app.UseSwaggerUI(options => // UseSwaggerUI is called only in Development.
//     {
//         options.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
//          options.RoutePrefix = string.Empty;
//     });
// }

// app.MapGet("/items", async (ToDoDbContext db) => await db.Items.ToListAsync());

// app.MapPost("/", async (ToDoDbContext db, string name) =>
// {
//     Item item = new Item();
//     item.Name = name;
//     item.Iscomplete=false;

//     await db.Items.AddAsync(item);
//     return await db.SaveChangesAsync();
// });

// app.MapDelete("{id}", async (ToDoDbContext db, int id) =>
// {
//     var item = await db.Items.FindAsync(id);
//     if (item == null) return Results.NotFound();
//     db.Items.Remove(item);
//     await db.SaveChangesAsync();
//     return Results.Ok();
// });

// app.MapPatch("{id}", async (ToDoDbContext db, int id,bool IsComplete) =>
// {
   
//     var itemToUpdate = await db.Items.FindAsync(id);
//     if (itemToUpdate == null) return Results.NotFound();
//     itemToUpdate.Iscomplete = IsComplete;
//     await db.SaveChangesAsync();
//     return Results.Ok();
// });

// app.MapGet("/",()=>"my server is running!!!");
// app.Run();






using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using TodoApi;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c => {
    c.SwaggerDoc("v1", new OpenApiInfo {
        Version = "v1"
    });
});

builder.Services.AddDbContext<ToDoDbContext>(options =>
    options.UseMySql(builder.Configuration.GetConnectionString("ToDoDB"), new MySqlServerVersion(new Version(8, 0, 41))));

builder.Services.AddCors(option => option.AddPolicy("AllowAll",
    p => p.AllowAnyOrigin()
          .AllowAnyMethod()
          .AllowAnyHeader()));

var app = builder.Build();

app.UseCors("AllowAll");

app.UseSwagger();
if (builder.Environment.IsDevelopment()) {
    app.UseSwaggerUI(options => {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
        options.RoutePrefix = string.Empty;
    });
}

// מסלול GET להבאת כל הפריטים
app.MapGet("/items", async (ToDoDbContext db) => await db.Items.ToListAsync());

// מסלול POST להוספת פריט חדש
app.MapPost("/", async (ToDoDbContext db, string name) => {
    Item item = new Item();
    item.Name = name;
    item.Iscomplete = false;

    await db.Items.AddAsync(item);
    await db.SaveChangesAsync();
    return Results.Ok(item);
});

// מסלול DELETE למחיקת פריט לפי מזהה
app.MapDelete("{id}", async (ToDoDbContext db, int id) => {
    var item = await db.Items.FindAsync(id);
    if (item == null) return Results.NotFound();
    db.Items.Remove(item);
    await db.SaveChangesAsync();
    return Results.Ok();
});

// מסלול PATCH לעדכון פריט לפי מזהה
app.MapPatch("{id}", async (ToDoDbContext db, int id, bool IsComplete) => {
    var itemToUpdate = await db.Items.FindAsync(id);
    if (itemToUpdate == null) return Results.NotFound();
    itemToUpdate.Iscomplete = IsComplete;
    await db.SaveChangesAsync();
    return Results.Ok();
});

// מסלול GET לבדיקת מצב השרת
app.MapGet("/", () => "my server is running!!!");

app.Run();
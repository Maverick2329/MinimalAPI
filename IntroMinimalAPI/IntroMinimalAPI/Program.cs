using IntroMinimalAPI;
using IntroMinimalAPI.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<ContactsDBContext>();

var app = builder.Build();

app.MapGet("/", () => "Hello World!");
app.MapGet("/brewery", () => new Repository().GetBreweries());
app.MapGet("/brewery/{id}", (int id) =>
{
    var brewery = new Repository().GetBrewery(id);
    return brewery == null? Results.NotFound() : Results.Ok(brewery);
});

//app.MapGet("/Contacts", () =>
//{
//    List<Contact> contacts = null;

//    using(var db = new ContactsDBContext())
//    {
//        contacts = db.Contacts.ToList();
//    }

//    return contacts;
//});

app.MapGet("/Contacts", (ContactsDBContext db) => db.Contacts.ToList());
app.MapPost("/Contacts",async (ContactsDBContext db, Contact contact) =>
{
    db.Contacts.Add(contact);
    await db.SaveChangesAsync();
    return Results.Created($"/Contacts/{contact.Id}", contact);
});
app.MapPut("/Contacts/{id}", async (int id, ContactsDBContext db, Contact contactRequest) =>
{
    var contact = await db.Contacts.FindAsync(id);
    if (contact is null) return Results.NotFound();

    contact.FirstName = contactRequest.FirstName;
    contact.LastName = contactRequest.LastName;
    contact.Phone = contactRequest.Phone;
    contact.Address = contactRequest.Address;

    await db.SaveChangesAsync();
    return Results.NoContent();
});
app.MapDelete("/Contacts/{id}", async (int id, ContactsDBContext db) =>
{
    var contact = await db.Contacts.FindAsync(id);
    if(contact is null) return Results.NotFound();
    db.Contacts.Remove(contact);
    await db.SaveChangesAsync();
    return Results.Ok(contact);
});

app.Run();

# Bible Verse Generator

A full stack web application that returns random Bible verses based on tags you select. Built with a clean, minimal UI and a RESTful API backend.


![Biblegenerator](https://github.com/user-attachments/assets/9ab80b25-725c-4a75-86cb-e66e9f98e441)


## Features

- Filter verses by one or multiple tags simultaneously
- Fully random verse selection from a seeded SQLite database
- Smooth loading animation on each new verse reveal
- Clean UI with responsive tag selection

## Tech Stack

**Backend**
- C# / ASP.NET Core Minimal API
- Entity Framework Core
- SQLite

**Frontend**
- HTML, CSS, JavaScript (vanilla)
- Tailwind CSS

## API Endpoints

### `GET /randomverse`
Returns a single random Bible verse. Optionally filter by one or more tags.

**Query Parameters**

| Parameter | Type | Description |
|-----------|------|-------------|
| `tag` | `string[]` | One or more tags to filter by. Verses must match all provided tags. |

**Example request**
```
GET /randomverse?tag=prayer&tag=wisdom
```

**Example response**
```json
{
  "id": 42,
  "reference": "James 1:5",
  "book": "James",
  "chapter": 1,
  "verse": 5,
  "text": "If any of you lacks wisdom, let him ask of God...",
  "tags": ["wisdom", "prayer", "new_testament"]
}
```

**Responses**
- `200 OK` — verse found
- `404 Not Found` — no verses match the provided tags

---

### `GET /verses`
Returns all Bible verses matching the provided tags.

**Query Parameters**

| Parameter | Type | Description |
|-----------|------|-------------|
| `tag` | `string[]` | One or more tags to filter by. |

**Example request**
```
GET /verses?tag=grace
```

**Responses**
- `200 OK` — list of matching verses
- `404 Not Found` — no verses match the provided tags

---

### Available Tags

`grace` `weakness` `power_of_God` `Christ` `Mary` `old_testament` `new_testament` `salvation` `sin` `prayer` `wisdom` `Holy_Spirit`

## Running Locally

**Prerequisites**
- [.NET 8 SDK](https://dotnet.microsoft.com/download)

**Steps**

1. Clone the repository
```bash
git clone https://github.com/yourusername/bible-verse-generator
cd bible-verse-generator
```

2. Run the API
```bash
cd BibleVerseGenerator
dotnet run
```
The API will start at `http://localhost:5186`

3. Open the frontend

Open `index.html` directly in your browser or use a simple local server:
```bash
cd frontend
npx serve .
```

4. Make sure the `API_URL` in your JavaScript matches the local API address:
```javascript
const API_URL = 'http://localhost:5186';
```

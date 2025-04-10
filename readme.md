
# PWT_SalesOrder

Project ini merupakan aplikasi fullstack yang terdiri dari:
- **Frontend**: Vue.js (dengan Vite, Typescript, Primevue, Tailwind CSS)
- **Backend**: ASP.NET Core 8.0 Web API

Struktur folder:
```
PWT_SalesOrder/
├── pwt_salesorder.client/       # Project Vue.js
├── PWT_SalesOrder.Server/       # Project ASP.NET Core (Web API + static file hosting)
└── PTW_SalesOrder.sln           # Solution file
```

---

## 📦 Persiapan

Pastikan sudah terinstal:
- [Visual Studio 2022](https://visualstudio.microsoft.com/vs/) dengan workload:
  - **ASP.NET and web development**
  - **Node.js development**
- [Node.js LTS](https://nodejs.org/en) (disarankan versi 22.x ke atas)
- .NET SDK 8.0

---

## 🏗 Build & Publish ke Folder

### 1. Build Vue dan ASP.NET Core sekaligus
Dari root solution folder (yang ada `.sln`-nya), jalankan:

```bash
dotnet publish PWT_SalesOrder.Server/PWT_SalesOrder.Server.csproj -c Release -o ./publish
```

> Ini akan:
> - Build Vue otomatis (via konfigurasi `.csproj`)
> - Copy hasil build Vue ke `wwwroot/`
> - Publish hasil ASP.NET Core ke folder `./publish`

---

## ▶ Menjalankan Hasil Publish

1. Masuk ke folder publish:
```bash
cd publish
```

2. Jalankan backend ASP.NET Core:
```bash
dotnet PWT_SalesOrder.Server.dll
```

3. Akses aplikasi di browser:
```
http://localhost:5000
```
---

## 🔧 Ubah Connection String

1. Buka file `appsettings.json` di folder `PWT_SalesOrder.Server`.
2. Temukan bagian:

```json
"ConnectionStrings": {
  "DefaultConnection": "Server=localhost;Database=namadb;Trusted_Connection=True;"
}
```

3. Ubah `Server`, `Database`, `User Id`, dan `Password` sesuai kebutuhan.

---

## 📁 Catatan Build

- Vue akan otomatis dibuild dan dicopy ke folder `wwwroot/` selama `dotnet publish`.
- Tidak perlu jalankan `npm run build` manual jika sudah diatur di `.csproj`.
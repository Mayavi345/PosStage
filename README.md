# PosStage POS System

## 專案簡介

PosStage 是一套以 .NET 所撰寫的 POS (Point of Sale) 系統，提供完整的前台與後台管理功能。前台可供店家進行日常銷售作業，後台則負責商品與員工管理、報表查詢等功能，同時亦提供 Web API 方便擴充與整合。

## 架構概覽

專案採用多層式架構，並以 `PosStage.sln` 將各模組串聯：

- **Stage.Data**：定義資料模型。
- **Stage.DAL**：資料存取層，實作 Repository 模式。
- **Stage.BLL**：商業邏輯層，透過 `ServiceFactory` 組合不同服務與資料庫。
- **Stage.Presentation**：前台 WPF 應用程式，採 MVVM 模式。
- **Stage.Backstage**：後台管理 WPF 應用程式，同樣以 MVVM 架構實作。
- **Stage.WebAPI**：提供 RESTful API，方便其他系統存取。
- **Stage.ReportViewCore** / **Stage.RepoertView**：使用 RDLC 報表呈現消費明細等資訊。
- **Stage.UnitTest**：NUnit 測試專案。
- **Utility**：包含共用的工具、設定與輔助類別。

## 設計模式與技術特色

- **Factory Pattern**：`ServiceFactory` 根據 `EDataDb` 與 `EServiceType` 建立對應的資料存取與服務實例，可在 MSSQL 與 SQLite 間切換，亦能選擇使用資料庫或 API 服務【F:Stage.BLL/BLL/ServiceFactory.cs†L30-L87】。
- **Singleton Pattern**：`MainSystemService` 為全域存取點，負責初始化系統與提供各項服務【F:Stage.BLL/BLL/MainSystemService.cs†L8-L35】。
- **Observer Pattern**：透過 `MessageSubject` 訂閱/通知機制，在 UI 與後端之間傳遞訊息【F:Utility/Structure/Observer/MessageSubject.cs†L1-L24】。
- **MVVM**：所有 WPF 專案皆以 MVVM 實作，例如 `MainWindowViewModel` 在啟動時初始化服務並透過 `PageManager` 切換畫面【F:Stage.Presentation/MVVM/MainWindowViewModel.cs†L16-L25】。
- **模組化頁面管理**：`PageHelper` 動態建立並註冊各頁面，方便日後擴充或替換【F:Stage.Presentation/Common/Page/PageHelper.cs†L16-L82】。

## 主要功能

- **前台作業**：提供商品瀏覽、加入購物車與結帳等功能。購物車邏輯集中在 `ShoppingCartService`，可計算總金額與項目數量【F:Stage.BLL/BLL/Service/Shop/ShoppingCartService.cs†L1-L92】。
- **後台管理**：後台可維護員工與商品資料，並透過 `BackstagePageHelper` 輕鬆切換各管理頁面。
- **報表產生**：`ReportService` 搭配 `ReportRepository` 讀取資料庫並產生銷售報表【F:Stage.BLL/BLL/Service/ReportService.cs†L1-L30】【F:Stage.DAL/Repositories/Implement/Report/ReportRepository.cs†L1-L59】。
- **Web API**：`Stage.WebAPI` 將商業邏輯對外提供，例如會員相關 API 由 `MemberController` 透過 `MainSystemService` 取得服務。
- **單元測試**：`Stage.UnitTest` 使用 NUnit 撰寫測試，確保核心功能正確。

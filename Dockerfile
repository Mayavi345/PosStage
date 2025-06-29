# syntax=docker/dockerfile:1

###############################################################################
# build 階段：還原並編譯所有相依專案
###############################################################################
FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src

# 1) 先複製所有 .csproj 檔，用以 layer cache
COPY Stage.BLL/Stage.BLL.csproj       Stage.BLL/
COPY Stage.Data/Stage.Data.csproj     Stage.Data/
COPY Stage.DAL/Stage.DAL.csproj       Stage.DAL/
COPY Utility/Utilities.csproj         Utility/
COPY Stage.WebMvc/Stage.WebMvc.csproj Stage.WebMvc/

# 2) 還原 MVC 專案 (它會自動帶入以上所有 ProjectReference)
RUN dotnet restore Stage.WebMvc/Stage.WebMvc.csproj

# 3) 複製所有專案原始碼（.cs、Controllers、Models、Views…）
COPY Stage.BLL/       Stage.BLL/
COPY Stage.Data/      Stage.Data/
COPY Stage.DAL/       Stage.DAL/
COPY Utility/         Utility/
COPY Stage.WebMvc/    Stage.WebMvc/

# 4) 發行 (publish) MVC 到 /app/publish
RUN dotnet publish Stage.WebMvc/Stage.WebMvc.csproj \
    -c Release \
    -o /app/publish

###############################################################################
# runtime 階段：執行已發行的 WebMvc
###############################################################################
FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS runtime
WORKDIR /app

# 5) 把 build 階段的發行結果拷貝過來
COPY --from=build /app/publish .

EXPOSE 80
ENTRYPOINT ["dotnet", "Stage.WebMvc.dll"]
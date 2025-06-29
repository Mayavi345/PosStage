#!/bin/bash
# ------------------------------------------------------------------------------
# restore.sh — 指定正確的 logical names 還原 PosDB.bak
# ------------------------------------------------------------------------------

# 改成備份內真正的 logical 檔名
MDF_LOGICAL="PosDB"
LDF_LOGICAL="PosDB_log"

echo "Using MDF logical name: $MDF_LOGICAL"
echo "Using LDF logical name: $LDF_LOGICAL"

# 還原資料庫
sqlcmd -S localhost -U sa -P "$SA_PASSWORD" -Q "\
  RESTORE DATABASE PosDB \
    FROM DISK = '/var/opt/mssql/backup/PosDB.bak' \
    WITH \
      MOVE N'$MDF_LOGICAL' TO N'/var/opt/mssql/data/PosDB.mdf', \
      MOVE N'$LDF_LOGICAL' TO N'/var/opt/mssql/data/PosDB_log.ldf', \
      REPLACE;
"

echo "Restore complete."
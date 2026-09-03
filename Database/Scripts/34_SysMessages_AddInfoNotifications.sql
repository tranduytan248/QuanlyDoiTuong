-- Script 34: Add missing Info notification messages to Sys_Messages
IF NOT EXISTS (SELECT 1 FROM Sys_Messages WHERE LabelKey = 'Info_Title' AND LangCode = 'vi-VN')
BEGIN
    INSERT INTO Sys_Messages (LangCode, LabelKey, Message) VALUES ('vi-VN', 'Info_Title', N'Thông báo');
END
ELSE
BEGIN
    UPDATE Sys_Messages SET Message = N'Thông báo' WHERE LabelKey = 'Info_Title' AND LangCode = 'vi-VN';
END

IF NOT EXISTS (SELECT 1 FROM Sys_Messages WHERE LabelKey = 'Common_UpdateInfo' AND LangCode = 'vi-VN')
BEGIN
    INSERT INTO Sys_Messages (LangCode, LabelKey, Message) VALUES ('vi-VN', 'Common_UpdateInfo', N'{0}');
END

IF NOT EXISTS (SELECT 1 FROM Sys_Messages WHERE LabelKey = 'Common_AddInfo' AND LangCode = 'vi-VN')
BEGIN
    INSERT INTO Sys_Messages (LangCode, LabelKey, Message) VALUES ('vi-VN', 'Common_AddInfo', N'{0}');
END

IF NOT EXISTS (SELECT 1 FROM Sys_Messages WHERE LabelKey = 'Common_DeleteInfo' AND LangCode = 'vi-VN')
BEGIN
    INSERT INTO Sys_Messages (LangCode, LabelKey, Message) VALUES ('vi-VN', 'Common_DeleteInfo', N'{0}');
END

IF NOT EXISTS (SELECT 1 FROM Sys_Messages WHERE LabelKey = 'Common_Info' AND LangCode = 'vi-VN')
BEGIN
    INSERT INTO Sys_Messages (LangCode, LabelKey, Message) VALUES ('vi-VN', 'Common_Info', N'{0}');
END

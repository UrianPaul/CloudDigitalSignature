#pragma once
#include "windows.h"

#define MYMENU_EXIT         (WM_APP + 101)
#define MYMENU_MESSAGEBOX   (WM_APP + 102) 
#define button1 1
#define IDC_BUTTON 1
#define IDC_USERNAME 2
#define IDC_USERNAMELABEL 3
#define IDC_PASSWORD 4
#define IDC_PASSWORDLABEL 5


LRESULT CALLBACK DLLWindowProc(HWND, UINT, WPARAM, LPARAM);

char*getpassword();

BOOL RegisterDLLWindowClass(wchar_t szClassName[]);

HMENU CreateDLLWindowMenu();

DWORD WINAPI ThreadProc(LPVOID lpParam);

LRESULT CALLBACK DLLWindowProc(HWND hwnd, UINT message, WPARAM wParam, LPARAM lParam);

LRESULT CALLBACK PasswordProc(HWND hDlg, UINT message, WPARAM wParam, LPARAM lParam);
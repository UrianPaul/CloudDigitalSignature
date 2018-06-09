#pragma once
#include"popUp.h"

HINSTANCE  inj_hModule;          //Injected Modules Handle
HWND       prnt_hWnd;
char username[30];
char password[30];


char *getpassword()
{
	return password;
}
void setPassword(char* passwd)
{
	strcpy(password, passwd);
}
char* convertToChar(wchar_t* wanted)
{
	char buffer[30];
	int len = wcslen(wanted);
	wcstombs(buffer, wanted, 30);
	return buffer;
}

//Register our windows Class
BOOL RegisterDLLWindowClass(wchar_t szClassName[])
{
	WNDCLASSEX wc;
	wc.hInstance = inj_hModule;
	wc.lpszClassName = (LPCWSTR)L"InjectedDLLWindowClass";
	wc.lpszClassName = (LPCWSTR)szClassName;
	wc.lpfnWndProc = DLLWindowProc;
	wc.style = CS_DBLCLKS;
	wc.cbSize = sizeof(WNDCLASSEX);
	wc.hIcon = LoadIcon(NULL, IDI_APPLICATION);
	wc.hIconSm = LoadIcon(NULL, IDI_APPLICATION);
	wc.hCursor = LoadCursor(NULL, IDC_ARROW);
	wc.lpszMenuName = NULL;
	wc.cbClsExtra = 0;
	wc.cbWndExtra = 0;
	wc.hbrBackground = (HBRUSH)COLOR_BACKGROUND;
	if (!RegisterClassEx(&wc))
		return 0;
}

HMENU CreateDLLWindowMenu()
{
	HMENU hMenu;
	hMenu = CreateMenu();
	HMENU hMenuPopup;
	if (hMenu == NULL)
		return FALSE;
	hMenuPopup = CreatePopupMenu();
	AppendMenu(hMenuPopup, MF_STRING, MYMENU_EXIT, TEXT("Exit"));
	AppendMenu(hMenu, MF_POPUP, (UINT_PTR)hMenuPopup, TEXT("File"));

	hMenuPopup = CreatePopupMenu();
	AppendMenu(hMenuPopup, MF_STRING, MYMENU_MESSAGEBOX, TEXT("MessageBox"));
	AppendMenu(hMenu, MF_POPUP, (UINT_PTR)hMenuPopup, TEXT("Test"));
	return hMenu;
}

DWORD WINAPI ThreadProc(LPVOID lpParam)
{
	MSG messages;
	wchar_t *pString = (wchar_t *) (lpParam);
	HMENU hMenu = CreateDLLWindowMenu();
	RegisterDLLWindowClass(L"InjectedDLLWindowClass");
	prnt_hWnd = FindWindow(L"Window Injected Into ClassName", L"Window Injected Into Caption");
	HWND hwnd = CreateWindowEx(WS_EX_CLIENTEDGE, L"InjectedDLLWindowClass", pString, WS_OVERLAPPEDWINDOW&~WS_MAXIMIZEBOX,
		CW_USEDEFAULT, CW_USEDEFAULT, 500, 350, prnt_hWnd, NULL, inj_hModule, NULL);
	ShowWindow(hwnd, SW_SHOWNORMAL);
	while (GetMessage(&messages, NULL, 0, 0))
	{
		TranslateMessage(&messages);
		DispatchMessage(&messages);
	}
	return 1;
}

//Our new windows proc
LRESULT CALLBACK DLLWindowProc(HWND hwnd, UINT message, WPARAM wParam, LPARAM lParam)
{
	wchar_t  pass[30];
	
	switch (message)
	{
	case WM_CREATE:
		//CreateWindow(L"Static", L"Username:", WS_VISIBLE | WS_CHILD, 10, 10, 80, 30, hwnd, (HMENU)IDC_USERNAMELABEL, NULL, NULL);
		//CreateWindow(L"Edit", L"", WS_VISIBLE | WS_CHILD, 100, 10, 250, 30, hwnd, (HMENU)IDC_USERNAME, NULL, NULL);
		CreateWindow(L"Static", L"Password:", WS_VISIBLE | WS_CHILD, 10, 100, 80, 30, hwnd, (HMENU)IDC_PASSWORDLABEL, NULL, NULL);
		CreateWindow(L"Edit", L"", ES_PASSWORD | WS_VISIBLE | WS_CHILD, 100, 100, 250, 30, hwnd, (HMENU)IDC_PASSWORD, NULL, NULL);
		CreateWindow(L"Button", L"Submit", WS_VISIBLE | WS_CHILD, 200, 200, 90, 35, hwnd, (HMENU)IDC_BUTTON, NULL, NULL);
		break;
	case WM_COMMAND:
		switch (wParam)
		{
		case  IDC_BUTTON:
			//GetWindowText(GetDlgItem(hwnd, IDC_USERNAME), (LPWSTR)username, sizeof(username));
			GetWindowText(GetDlgItem(hwnd, IDC_PASSWORD), pass, sizeof(pass));
			//setPassword(pass);
			strcpy(password , convertToChar(pass));
			//	MessageBox(hwnd, (LPWSTR)username, L"aaa", MB_OK);
			//MessageBox(hwnd, (LPWSTR)password, L"aaa", MB_OK);
			PostQuitMessage(0);
			break;
		}
		break;
	case WM_DESTROY:
		PostQuitMessage(0);
		break;
	default:
		return DefWindowProc(hwnd, message, wParam, lParam);
	}
	return 0;
}

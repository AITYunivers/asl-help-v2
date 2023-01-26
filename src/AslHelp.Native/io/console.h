#pragma once

#include "../pch.h"

HANDLE console;

BOOL InitConsole(void);
BOOL Log(const char* output);
BOOL LogDw(const char* format, DWORD value);
BOOL LogLastErr(const char* msg);
BOOL DisposeConsole(void);

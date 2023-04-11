#pragma once

#include "../pch.h"

void* s_console;

bool InitConsole(void);
bool Log(const char* output);
bool LogDw(const char* format, u32 value);
bool LogLastErr(const char* msg);
bool DisposeConsole(void);

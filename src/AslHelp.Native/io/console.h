#pragma once

#include <stdarg.h>
#include <string.h>
#include "../pch.h"

void* s_console;

bool InitConsole(void);
void Log(const char* cmd, const char* format, ...);
bool DisposeConsole(void);

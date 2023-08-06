#pragma once

#include "PipeRequest.hxx"
#include "PipeResponse.hxx"

#include "DebugLogger.hxx"
#include "NamedPipeServer.hxx"

#ifdef DEBUG
#define DEBUG_LOG(format, ...) IO::DebugLogger::Log(format, __VA_ARGS__)
#else
#define DEBUG_LOG(format, ...)
#endif

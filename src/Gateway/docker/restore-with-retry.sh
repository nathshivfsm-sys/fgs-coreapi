#!/bin/sh
set -eu

if [ "$#" -lt 1 ]; then
  echo "Usage: restore-with-retry.sh <project-path> [additional dotnet restore args...]" >&2
  exit 1
fi

project="$1"
shift

attempt=1
max_attempts=5

while [ "$attempt" -le "$max_attempts" ]; do
  echo "dotnet restore attempt ${attempt}/${max_attempts} for ${project}"
  if dotnet restore "$project" --disable-parallel "$@"; then
    exit 0
  fi

  if [ "$attempt" -eq "$max_attempts" ]; then
    echo "dotnet restore failed after ${max_attempts} attempts for ${project}" >&2
    exit 1
  fi

  sleep_seconds=$((attempt * 15))
  echo "Restore failed; waiting ${sleep_seconds}s before retry..."
  sleep "$sleep_seconds"
  attempt=$((attempt + 1))
done

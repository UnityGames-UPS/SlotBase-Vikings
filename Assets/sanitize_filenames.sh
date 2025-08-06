#!/bin/bash

# Enable nullglob to avoid issues with unexpanded globs
shopt -s nullglob

echo "🔍 Scanning for Windows-incompatible filenames..."

# Windows-forbidden characters
RESERVED_CHARS='[<>:"/\\|?*]'

# Perform depth-first traversal to avoid issues when renaming directories
find . -depth | while IFS= read -r path; do
    dir=$(dirname "$path")
    base=$(basename "$path")
    sanitized="$base"

    # Remove trailing spaces and periods
    sanitized=$(echo "$sanitized" | sed 's/[ .]*$//')

    # Remove Windows reserved characters (safe delimiter: | )
    sanitized=$(echo "$sanitized" | sed 's|[<>:"/\\|?*]||g')

    # Skip if sanitized name is empty
    if [[ -z "$sanitized" ]]; then
        echo "⚠️  Skipping '$path' (sanitized name is empty)"
        continue
    fi

    # Skip if no change
    if [[ "$base" == "$sanitized" ]]; then
        continue
    fi

    old_path="$path"
    new_path="$dir/$sanitized"

    # Skip if the destination already exists
    if [[ -e "$new_path" ]]; then
        echo "⚠️  Conflict: '$new_path' already exists. Skipping rename of '$old_path'."
        continue
    fi

    echo "🔁 Renaming: '$old_path' -> '$new_path'"
    git mv -f "$old_path" "$new_path"
done

echo "✅ Done. Review changes with 'git status'."


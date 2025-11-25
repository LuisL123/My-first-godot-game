
Game idea:
Simple Binding of Isaac inspired dungeon-crawler where the player has to find the key and escape through the lock door without dying


Game asset: https://trevor-pupkin.itch.io/tech-dungeon-roguelite

# 🕹️ Godot Dungeon Crawler — Dev Checklist

## 🧱 Setup
- [x] Create folders: `scenes/actors`, `scenes/levels`, `ui`, `assets`, `scripts`
- [x] Add Input Map: `move_up/down/left/right`, `shoot`
- [x] Make `Level01.tscn` → add `TileMap` + collision tiles

---

## 🧍‍♂️ Player
- [x] Create `Player.tscn` (CharacterBody2D + Sprite + Collision)
- [x] Add script: 8-dir movement + `Camera2D` follow
- [ ] Add HP vars, `apply_damage()`, and death → `LoseScreen`
- [x] Group player under `"player"`
- [x] Add gun that rotates

---

## 👾 Enemy
- [x] Create `EnemyGrunt.tscn` (CharacterBody2D + Sprite + Collision)
- [x] Script: chase player → damage on contact & push in opposite direction
- [x] Add `apply_damage()` + death animation or `queue_free()`
- [ ] Fix bug where enemy matches player speed.

---

## ⚔️ Combat
- [x] Ranged: spawn `Bullet.tscn` (Area2D) toward aim dir
- [ ] Add an ammunition system: Limited bullets, enemies bullets.

---

## ❤️ HUD
- [ ] Create `HUD.tscn` (CanvasLayer + Label/Icons)
- [ ] Connect `player.hp_changed` → HUD update
- [ ] Add `LoseScreen.tscn` with “Retry” button → reload level

---

## 🗝️ Objective
- [ ] `KeyItem.tscn`: on pickup → `player.has_key = true`
- [ ] `DoorExit.tscn`: if `has_key` → load `WinScreen.tscn`
- [ ] Place key + enemies guarding it

---

## ✨ Polish
- [ ] Add SFX (hit, pickup, shoot)
- [ ] Camera shake + sprite flash on damage
- [ ] Pause menu (`Esc` toggles overlay)
- [ ] Export build → test Win & Lose paths

---

## ✅ Done When
- [ ] Can walk, fight, pick up key, exit → Win
- [ ] Can die → Lose
- [ ] No console errors
- [ ] Full loop playable in < 5 min

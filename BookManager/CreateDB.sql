-- ジャンル
create table genre (
    -- ジャンル
	genrename   TEXT  primary key
);

-- 段ボール
create table box (
    -- 段ボール
	boxname     TEXT    primary key
);

-- 配置
create table position (
    -- 配置
	position    TEXT   primary key
);

-- 蔵書
create table book (
    -- ID
	bookid      TEXT    primary  key,
	-- 書名
	bookname    TEXT,
	-- 著者名
	author      TEXT,
	-- ジャンル
	genre       TEXT    references genre(genrename) ON DELETE NO ACTION,
	-- 配置
	position    TEXT    references position(position) ON DELETE NO ACTION,
	-- 所属段ボール
	box         TEXT    references box(boxname)  ON DELETE NO ACTION
);

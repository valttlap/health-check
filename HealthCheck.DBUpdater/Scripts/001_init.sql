CREATE EXTENSION IF NOT EXISTS "uuid-ossp";
CREATE SCHEMA IF NOT EXISTS health_check;

CREATE TABLE health_check.category (
    id INT PRIMARY KEY,
    title_fi TEXT NOT NULL,
    title_en TEXT NOT NULL,
    example_good_fi TEXT NOT NULL,
    example_good_en TEXT NOT NULL,
    example_bad_fi TEXT NOT NULL,
    example_bad_en TEXT NOT NULL
);

CREATE TABLE health_check.session (
    id UUID PRIMARY KEY DEFAULT uuid_generate_v4(),
    join_code INT NOT NULL UNIQUE,
    current_category_id INT NOT NULL DEFAULT 1,
    FOREIGN KEY (current_category_id)
        REFERENCES health_check.category(id)
        ON UPDATE CASCADE
        ON DELETE RESTRICT
);

CREATE TABLE health_check.session_user (
    id UUID PRIMARY KEY DEFAULT uuid_generate_v4(),
    session_id UUID NOT NULL,
    FOREIGN KEY (session_id)
        REFERENCES health_check.session(id)
        ON UPDATE CASCADE
        ON DELETE CASCADE
);

CREATE TABLE health_check.vote (
    id UUID PRIMARY KEY DEFAULT uuid_generate_v4(),
    category_id INT NOT NULL,
    vote_value INT NOT NULL,
    user_id UUID NOT NULL,
    session_id UUID NOT NULL,
    UNIQUE(category_id, user_id, session_id),
    FOREIGN KEY (category_id)
        REFERENCES health_check.category(id)
        ON UPDATE CASCADE
        ON DELETE RESTRICT,
    FOREIGN KEY (user_id)
        REFERENCES health_check.session_user(id)
        ON UPDATE CASCADE
        ON DELETE CASCADE,
    FOREIGN KEY (session_id)
        REFERENCES health_check.session(id)
        ON UPDATE CASCADE
        ON DELETE CASCADE
);

INSERT INTO health_check.category (
    id,
    title_fi,
    title_en,
    example_good_fi,
    example_good_en,
    example_bad_fi,
    example_bad_en
)
VALUES
(1, 'Tuki', 'Support',
 'Saamme aina erinomaista tukea ja apua, kun pyydämme sitä!',
 'We always get great support & help when we ask for it!',
 'Jäämme jatkuvasti jumiin, koska emme saa tarvitsemaamme tukea ja apua.',
 'We keep getting stuck because we can’t get the support & help that we ask for.'),

(2, 'Yhteistyö', 'Teamwork',
 'Olemme tiivis supertiimi, jossa on upeaa yhteistyötä!',
 'We are a totally gelled super-team with awesome collaboration!',
 'Olemme joukko yksilöitä, joilla ei ole mitään tietoa tai kiinnostusta siitä, mitä muut tiimissä tekevät.',
 'We are a bunch of individuals that neither know nor care about what the other people in the squad are doing.'),

(3, 'Sotilaita vai Pelureita', 'Pawns or players',
 'Meillä on kohtalomme omissa käsissämme! Me päätämme, mitä rakennamme ja miten sen toteutamme.',
 'We are in control of our destiny! We decide what to build and how to build it.',
 'Olemme vain shakkipelin sotilaita, joilla ei ole vaikutusvaltaa siihen, mitä rakennamme tai miten sen toteutamme.',
 'We are just pawns in a game of chess, with no influence over what we build or how we build it.'),

(4, 'Missio', 'Mission',
 'Tiedämme tarkalleen, miksi olemme täällä, ja olemme todella innoissamme siitä.',
 'We know exactly why we are here, and we are really excited about it',
 'Emme tiedä lainkaan, miksi olemme täällä. Kokonaiskuva ja fokus puuttuvat täysin, ja niin sanottu missiomme on täysin epäselvä ja innostamaton.',
 'We have no idea why we are here, there is no high level picture or focus. Our so-called mission is completely unclear and uninspiring.'),

(5, 'Koodipohjan terveys', 'Health of Codebase',
 'Olemme ylpeitä koodimme laadusta! Se on selkeää, helposti luettavaa ja hyvin testattua.',
 'We’re proud of the quality of our code! It is clean, easy to read, and has great test coverage.',
 'Koodimme on silkkaa sotkua, ja tekninen velka on karannut täysin hallinnasta.',
 'Our code is a pile of dung, and technical debt is raging out of control'),

(6, 'Arvon tuottaminen', 'Delivering Value',
 'Toimitamme erinomaista sisältöä! Olemme ylpeitä siitä, ja sidosryhmämme ovat erittäin tyytyväisiä.',
 'We deliver great stuff! We’re proud of it and our stakeholders are really happy.',
 'Toimitamme roskaa. Meitä hävettää toimittaa sitä. Sidosryhmämme vihaavat meitä.',
 'We deliver crap. We feel ashamed to deliver it. Our stakeholders hate us.'),

(7, 'Oppiminen', 'Learning',
 'Opimme jatkuvasti paljon mielenkiintoisia asioita!',
 'We’re learning lots of interesting stuff all the time!',
 'Meillä ei ole koskaan aikaa oppia mitään uutta.',
 'We never have time to learn anything'),

(8, 'Nopeus', 'Speed',
 'Saamme asioita valmiiksi todella nopeasti. Ei odottelua, ei viivästyksiä.',
 'We get stuff done really quickly. No waiting, no delays.',
 'Emme tunnu saavan mitään valmiiksi. Joudumme jatkuvasti jumiin tai keskeytyksiksi. Tehtävät pysähtyvät riippuvuuksiin.',
 'We never seem to get done with anything. We keep getting stuck or interrupted. Stories keep getting stuck on dependencies'),

(9, 'Helppo julkaista', 'Easy to release',
 'Julkaiseminen on yksinkertaista, turvallista, kivutonta ja enimmäkseen automatisoitua.',
 'Releasing is simple, safe, painless & mostly automated.',
 'Julkaiseminen on riskialtista, tuskallista, vaatii paljon käsityötä ja kestää ikuisuuden.',
 'Releasing is risky, painful, lots of manual work, and takes forever.'),

(10, 'Hauskuus', 'Fun',
 'Työhön on mahtava tulla, ja meillä on hauskaa yhdessä.',
 'We love going to work, and have great fun working together',
 'Tylsääääää.',
 'Boooooooring.');

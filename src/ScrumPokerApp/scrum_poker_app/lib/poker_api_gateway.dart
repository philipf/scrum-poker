import 'package:http/http.dart' as http;
import 'dart:convert';


class PokerApiGateway {

  static const baseUri = 'scrum-poker-api.azurewebsites.net';

  Future<Session> createSession(String name) async {
    final Uri uri = Uri.https(baseUri, 'api/session',
     {
      'facilitatorName': name
     });

    final response = await http.post(uri);

    if (response.statusCode == 200) {
      final sessionJson = json.decode(response.body);
      Session session = Session.fromJson(sessionJson);
      return session;
    } else {
       throw Exception(response.body);
    }

  } 

}


class Session {
  String sessionId;
  String sessionName;
  List<Round> rounds;
  List<Participant> participants;
  List<String> votingScale;
  Round currentRound;

  Session({
    required this.sessionId,
    required this.sessionName,
    required this.rounds,
    required this.participants,
    required this.votingScale,
    required this.currentRound,
  });

  factory Session.fromJson(Map<String, dynamic> json) {
    return Session(
      sessionId: json['sessionId'],
      sessionName: json['sessionName'],
      rounds: List<Round>.from(json['rounds'].map((x) => Round.fromJson(x))),
      participants: List<Participant>.from(json['participants'].map((x) => Participant.fromJson(x))),
      votingScale: List<String>.from(json['votingScale'].map((x) => x)),
      currentRound: Round.fromJson(json['currentRound']),
    );
  }
}

class Round {
  int roundId;
  List<String> votingScale;
  List<Vote> votes;

  Round({
    required this.roundId,
    required this.votingScale,
    required this.votes,
  });

  factory Round.fromJson(Map<String, dynamic> json) {
    return Round(
      roundId: json['roundId'],
      votingScale: List<String>.from(json['votingScale'].map((x) => x)),
      votes: List<Vote>.from(json['votes'].map((x) => Vote.fromJson(x))),
    );
  }
}

class Vote {
  String? value;
  Participant participant;
  DateTime voteTime;

  Vote({
    required this.value,
    required this.participant,
    required this.voteTime,
  });

  factory Vote.fromJson(Map<String, dynamic> json) {
    return Vote(
      value: json['value'],
      participant: Participant.fromJson(json['participant']),
      voteTime: DateTime.parse(json['voteTime']),
    );
  }
}

class Participant {
  bool isFacilitator;
  bool isObserver;
  String id;
  DateTime joinedTime;
  String name;

  Participant({
    required this.isFacilitator,
    required this.isObserver,
    required this.id,
    required this.joinedTime,
    required this.name,
  });

  factory Participant.fromJson(Map<String, dynamic> json) {
    return Participant(
      isFacilitator: json['isFacilitator'],
      isObserver: json['isObserver'],
      id: json['id'],
      joinedTime: DateTime.parse(json['joinedTime']),
      name: json['name'],
    );
  }
}

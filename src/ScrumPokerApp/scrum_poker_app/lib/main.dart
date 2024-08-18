import 'package:flutter/material.dart';
import 'package:scrum_poker_app/poker_api_gateway.dart';

void main() {
  runApp(const MainApp());
}

class MainApp extends StatefulWidget {
  const MainApp({super.key});

  @override
  State<MainApp> createState() => _MainAppState();
}

class _MainAppState extends State<MainApp> {
  //Session? session;
  String sessionId = 'init';

  @override
  void initState() {
    super.initState();
  }

  @override
  Widget build(BuildContext context) {
    return MaterialApp(
      debugShowCheckedModeBanner: true,
      home: Scaffold(
          body: Center(
            child: Text('Hello World: $sessionId'),
          ),
          floatingActionButton: FloatingActionButton(onPressed: () {
            _createSession().then((value) {
              print(value.sessionId);
              setState(() {
                sessionId = value.sessionId;  
              });
            },
            ); 
          },
          child: const Icon(Icons.create),)),
    );
  }

  Future<Session> _createSession() async {
    final api = PokerApiGateway();
    final session = await api.createSession('test');
    return session;
  }
}

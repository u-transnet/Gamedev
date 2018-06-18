package com.github.utransnet.utranscalc.server;

import com.github.utransnet.utranscalc.server.transport.NetworkFacade;
import io.netty.bootstrap.ServerBootstrap;
import io.netty.channel.EventLoopGroup;
import io.netty.channel.nio.NioEventLoopGroup;
import io.netty.channel.socket.nio.NioServerSocketChannel;
import io.netty.handler.logging.LogLevel;
import io.netty.handler.logging.LoggingHandler;
import lombok.extern.slf4j.Slf4j;
import org.springframework.stereotype.Component;

import javax.annotation.PostConstruct;
import java.util.concurrent.ExecutorService;
import java.util.concurrent.Executors;

/**
 * Created by Artem on 23.05.2018.
 */
@Slf4j
@Component
public class Server {
    static final int PORT = Integer.parseInt(System.getProperty("port", "8463"));
    private final NetworkFacade networkFacade;

    public Server(NetworkFacade networkFacade) {
        this.networkFacade = networkFacade;
    }

    @PostConstruct
    public void init() {
        new Thread(() -> {

            // Create event loop groups. One for incoming connections handling and
            // second for handling actual event by workers
            EventLoopGroup serverGroup = new NioEventLoopGroup(1);
            EventLoopGroup workerGroup = new NioEventLoopGroup();
            ExecutorService businessLogicThreadPool = Executors.newCachedThreadPool();

            try {
                ServerBootstrap bootStrap = new ServerBootstrap();
                bootStrap.group(serverGroup, workerGroup)
                        .channel(NioServerSocketChannel.class)
                        .handler(new LoggingHandler(LogLevel.INFO))
                        .childHandler(new ServerChannelInitializer(businessLogicThreadPool, networkFacade));

                // Bind to port
                bootStrap.bind(PORT).sync().channel().closeFuture().sync();
                log.info("Server started on port " + PORT);
            } catch (InterruptedException e) {
                e.printStackTrace();
            } finally {
                serverGroup.shutdownGracefully();
                workerGroup.shutdownGracefully();
            }
        }).start();
    }
}
